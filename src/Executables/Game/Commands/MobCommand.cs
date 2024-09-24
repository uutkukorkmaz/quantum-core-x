using System.Security.Cryptography;
using CommandLine;
using Microsoft.Extensions.Logging;
using QuantumCore.API;
using QuantumCore.API.Game;
using QuantumCore.API.Game.World;
using QuantumCore.Game.Services;
using QuantumCore.Game.World.Entities;

namespace QuantumCore.Game.Commands
{
    public class MobCommandOptions
    {
        [Value(0, Required = true)] public uint MobId { get; set; }
    }

    [Command("mob", "Spawns a mob")]
    [Command("m", "Spawns a mob")]
    public class MobCommand : ICommandHandler<MobCommandOptions>
    {
        private readonly IMonsterManager _monsterManager;
        private readonly IDropProvider _dropProvider;
        private readonly IAnimationManager _animationManager;
        private readonly ILogger<MobCommand> _logger;
        private readonly IItemManager _itemManager;
        
        private const int SpawnRadius = 200;

        public MobCommand(
            IMonsterManager monsterManager,
            IDropProvider dropProvider,
            IAnimationManager animationManager,
            ILogger<MobCommand> logger,
            IItemManager itemManager
        )
        {
            _monsterManager = monsterManager;
            _dropProvider = dropProvider;
            _animationManager = animationManager;
            _logger = logger;
            _itemManager = itemManager;
        }

        public Task ExecuteAsync(CommandContext<MobCommandOptions> ctx)
        {
            var map = ctx.Player.Map;
            if (map == null)
            {
                ctx.Player.SendChatInfo("You are not in a map");
                return Task.CompletedTask;
            }

            var mob = _monsterManager.GetMonster(ctx.Arguments.MobId);
            if (mob == null)
            {
                ctx.Player.SendChatInfo("Mob not found");
                return Task.CompletedTask;
            }

            var spawnX = ctx.Player.PositionX + RandomNumberGenerator.GetInt32(-SpawnRadius, SpawnRadius);
            var spawnY = ctx.Player.PositionY + RandomNumberGenerator.GetInt32(-SpawnRadius, SpawnRadius);

            if (!map.IsPositionInside(spawnX, spawnY))
            {
                _logger.LogDebug(
                    "MobCommand - out of map boundries: {x} {y}. Using player position for spawn coordinates",
                    spawnX, spawnY
                );
                spawnX = ctx.Player.PositionX;
                spawnY = ctx.Player.PositionY;
            }

            map.SpawnEntity(new MonsterEntity(
                monsterManager: _monsterManager,
                dropProvider: _dropProvider,
                animationManager: _animationManager,
                map: map,
                logger: _logger,
                itemManager: _itemManager,
                id: ctx.Arguments.MobId,
                x: spawnX,
                y: spawnY,
                rotation: ctx.Player.Rotation
            ));

            return Task.CompletedTask;
        }
    }
}