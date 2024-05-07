﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QuantumCore.Game.Persistence.Entities;

[Table("perm_groups")]
public class PermGroup
{
    public required Guid Id { get; set; }
    [StringLength(30)] public required string Name { get; set; }

    public ICollection<PermAuth> Permissions { get; set; } = null!;
    public ICollection<PermUser> Users { get; set; } = null!;

    public static void Configure(EntityTypeBuilder<PermGroup> builder, DatabaseFacade database)
    {
        builder.HasIndex(x => x.Name).IsUnique();
        if (database.IsNpgsql())
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");
        }
    }
}