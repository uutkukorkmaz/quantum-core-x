﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using QuantumCore.API;
using QuantumCore.API.Game.World;
using QuantumCore.API.PluginTypes;
using QuantumCore.Core.Cache;
using QuantumCore.Core.Utils;
using QuantumCore.Database;
using QuantumCore.Game.Packets;
using QuantumCore.Game.PlayerUtils;

namespace QuantumCore.Game.PacketHandlers.Select;

public class CreateCharacterHandler : IGamePacketHandler<CreateCharacter>
{
    private readonly ILogger<CreateCharacterHandler> _logger;
    private readonly IDatabaseManager _databaseManager;
    private readonly IJobManager _jobManager;
    private readonly ICacheManager _cacheManager;
    private readonly IWorld _world;

    public CreateCharacterHandler(ILogger<CreateCharacterHandler> logger, IDatabaseManager databaseManager, 
        IJobManager jobManager, ICacheManager cacheManager, IWorld world)
    {
        _logger = logger;
        _databaseManager = databaseManager;
        _jobManager = jobManager;
        _cacheManager = cacheManager;
        _world = world;
    }
    
    public async Task ExecuteAsync(GamePacketContext<CreateCharacter> ctx, CancellationToken token = default)
    {
        _logger.LogDebug("Create character in slot {Slot}", ctx.Packet.Slot);
        if (ctx.Connection.AccountId == null)
        {
            ctx.Connection.Close();
            _logger.LogWarning("Character create received before authorization");
            return;
        }

        var accountId = ctx.Connection.AccountId ?? default;

        var db = _databaseManager.GetGameDatabase();
        var count = await db.QuerySingleAsync<int>("SELECT COUNT(*) FROM players WHERE Name = @Name", new {Name = ctx.Packet.Name});
        if (count > 0)
        {
            await ctx.Connection.Send(new CreateCharacterFailure());
            return;
        }

        var job = _jobManager.Get((byte)ctx.Packet.Class);
        
        // Create player data
        var player = new Player
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            Name = ctx.Packet.Name,
            PlayerClass = (byte) ctx.Packet.Class,
            PositionX = 958870,
            PositionY = 272788,
            St = job.St,
            Iq = job.Iq, 
            Dx = job.Dx, 
            Ht = job.Ht,
            Health =  job.StartHp, 
            Mana = job.StartSp,
            MaxHealth = job.StartHp + job.HpPerHt * job.Ht + job.HpPerLevel,
            MaxMana = job.StartSp + job.SpPerIq * job.Iq + job.SpPerLevel
        };


        // Persist player
        await _databaseManager.GetGameDatabase().InsertAsync(player);
        
        // Add player to cache
        await _cacheManager.Set("player:" + player.Id, player);
        
        // Add player to the list of characters
        var list = _cacheManager.CreateList<Guid>("players:" + accountId);
        var idx = await list.Push(player.Id);
        
        // Query responsible host for the map
        var host = _world.GetMapHost(player.PositionX, player.PositionY);
        
        // Send success response
        var character = Character.FromEntity(player);
        character.Ip = IpUtils.ConvertIpToUInt(host.Ip);
        character.Port = host.Port;
        await ctx.Connection.Send(new CreateCharacterSuccess
        {
            Slot = (byte)(idx - 1),
            Character = character
        });
    }
}