using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuantumCore.API;
using QuantumCore.API.Game;
using QuantumCore.API.Game.Types;
using QuantumCore.API.Game.World;
using QuantumCore.API.PluginTypes;
using QuantumCore.Core.Event;
using QuantumCore.Core.Networking;
using QuantumCore.Core.Utils;
using QuantumCore.Extensions;
using QuantumCore.Networking;

namespace QuantumCore.Game
{
    public class GameServer : ServerBase<GameConnection>, IGame, IGameServer
    {
        public static readonly Meter Meter = new Meter("QuantumCore:Game");
        private readonly Histogram<double> _serverTimes = Meter.CreateHistogram<double>("TickTime", "ms");
        private readonly HostingOptions _hostingOptions;
        private readonly ILogger<GameServer> _logger;
        private readonly PluginExecutor _pluginExecutor;
        private readonly ICommandManager _commandManager;
        private readonly IQuestManager _questManager;
        private readonly IChatManager _chatManager;
        public IWorld World { get; }

        private readonly Stopwatch _gameTime = new Stopwatch();
        private long _previousTicks = 0;
        private TimeSpan _accumulatedElapsedTime;
        private TimeSpan _targetElapsedTime = TimeSpan.FromTicks(100000); // 100hz
        private TimeSpan _maxElapsedTime = TimeSpan.FromMilliseconds(500);
        private readonly Stopwatch _serverTimer = new();
        private readonly ISessionManager _sessionManager;
        private readonly IEnumerable<ILoadable> _loadables;

        public new ImmutableArray<IGameConnection> Connections =>
            [..base.Connections.Values.Cast<IGameConnection>()];

        public static GameServer Instance { get; private set; } = null!; // singleton

        public GameServer(IOptions<HostingOptions> hostingOptions, IPacketManager packetManager,
            ILogger<GameServer> logger, PluginExecutor pluginExecutor, IServiceProvider serviceProvider,
            ICommandManager commandManager, IQuestManager questManager, IChatManager chatManager, IWorld world,
            ISessionManager sessionManager, IEnumerable<ILoadable> loadables)
            : base(packetManager, logger, pluginExecutor, serviceProvider, "game", hostingOptions)
        {
            _hostingOptions = hostingOptions.Value;
            _logger = logger;
            _pluginExecutor = pluginExecutor;
            _commandManager = commandManager;
            _questManager = questManager;
            _chatManager = chatManager;
            _sessionManager = sessionManager;
            _loadables = loadables;
            World = world;
            Instance = this;
            Meter.CreateObservableGauge("Connections", () => Connections.Length);
        }

        private void Update(double elapsedTime)
        {
            EventSystem.Update(elapsedTime);

            World.Update(elapsedTime);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Set public ip address
            if (_hostingOptions.IpAddress != null)
            {
                IpUtils.PublicIP = IPAddress.Parse(_hostingOptions.IpAddress);
            }
            else if (IpUtils.PublicIP is null)
            {
                // Query interfaces for our best ipv4 address
                IpUtils.SearchPublicIp();
            }

            // Load game data
            await Task.WhenAll(_loadables.Select(x => x.LoadAsync(stoppingToken)));

            // Initialize session manager
            _sessionManager.Init(this);

            // Initialize core systems
            _chatManager.Init();

            // Load all quests
            _questManager.Init();

            // Load game world
            _logger.LogInformation("Initialize world");
            await World.Load();

            // Register all default commands
            _commandManager.Register("QuantumCore.Game.Commands", Assembly.GetExecutingAssembly());
            _commandManager.Register("QuantumCore.Game.Commands.Guild", Assembly.GetExecutingAssembly());

            // Put all new connections into login phase
            RegisterNewConnectionListener(connection =>
            {
                connection.SetPhase(EPhases.Login);
                return true;
            });

            // Start server timer
            _serverTimer.Start();

            _logger.LogInformation("Start listening for connections...");

            StartListening();

            _gameTime.Start();

            _logger.LogDebug("Start!");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _pluginExecutor.ExecutePlugins<IGameTickListener>(_logger,
                        x => x.PreUpdateAsync(stoppingToken));
                    await Tick();
                    await _pluginExecutor.ExecutePlugins<IGameTickListener>(_logger,
                        x => x.PostUpdateAsync(stoppingToken));
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Tick failed");
                }
            }
        }

        private async ValueTask Tick()
        {
            var currentTicks = _gameTime.Elapsed.Ticks;
            var elapsedTime = TimeSpan.FromTicks(currentTicks - _previousTicks);
            _serverTimes.Record(elapsedTime.TotalMilliseconds);
            _accumulatedElapsedTime += elapsedTime;
            _previousTicks = currentTicks;

            if (_accumulatedElapsedTime < _targetElapsedTime)
            {
                var sleepTime = (_targetElapsedTime - _accumulatedElapsedTime).TotalMilliseconds;
                await Task.Delay((int) sleepTime).ConfigureAwait(false);
                return;
            }

            if (_accumulatedElapsedTime > _maxElapsedTime)
            {
                _logger.LogWarning($"Server is running slow");
                _accumulatedElapsedTime = _maxElapsedTime;
            }

            var stepCount = 0;
            while (_accumulatedElapsedTime >= _targetElapsedTime)
            {
                _accumulatedElapsedTime -= _targetElapsedTime;
                ++stepCount;

                //_logger.LogDebug($"Update... ({stepCount})");
                Update(_targetElapsedTime.TotalMilliseconds);
            }

            // todo detect lags
        }

        public void RegisterCommandNamespace(Type t)
        {
            _commandManager.Register(t.Namespace!, t.Assembly);
        }
    }
}
