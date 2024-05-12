﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuantumCore.Game.Persistence;

#nullable disable

namespace QuantumCore.Game.Persistence.Migrations.Sqlite
{
    [DbContext(typeof(SqliteGameDbContext))]
    [Migration("20240512163021_CharacterPlayToULong")]
    partial class CharacterPlayToULong
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("QuantumCore.Game.Persistence.Entities.DeletedPlayer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("TEXT");

                    b.Property<int>("BodyPart")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("current_timestamp");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("TEXT");

                    b.Property<byte>("Dx")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Experience")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Gold")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HairPart")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Health")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Ht")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Iq")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Mana")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("TEXT");

                    b.Property<int>("PlayTime")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("PlayerClass")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PositionX")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PositionY")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("SkillGroup")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("St")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Stamina")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("current_timestamp");

                    b.HasKey("Id");

                    b.ToTable("deleted_players");
                });

            modelBuilder.Entity("QuantumCore.Game.Persistence.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<byte>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("current_timestamp");

                    b.Property<uint>("ItemId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<uint>("Position")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("current_timestamp");

                    b.Property<byte>("Window")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("items");
                });

            modelBuilder.Entity("QuantumCore.Game.Persistence.Entities.PermAuth", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Command")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("perm_auth");
                });

            modelBuilder.Entity("QuantumCore.Game.Persistence.Entities.PermGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("perm_groups");

                    b.HasData(
                        new
                        {
                            Id = new Guid("45bff707-1836-42b7-956d-00b9b69e0ee0"),
                            Name = "Operator"
                        });
                });

            modelBuilder.Entity("QuantumCore.Game.Persistence.Entities.PermUser", b =>
                {
                    b.Property<Guid>("GroupId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.HasKey("GroupId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("perm_users");
                });

            modelBuilder.Entity("QuantumCore.Game.Persistence.Entities.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("TEXT");

                    b.Property<uint>("AvailableStatusPoints")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("BodyPart")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("current_timestamp");

                    b.Property<byte>("Dx")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Empire")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("Experience")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("GivenStatusPoints")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("Gold")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("HairPart")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Health")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Ht")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Iq")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Mana")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("TEXT");

                    b.Property<ulong>("PlayTime")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("PlayerClass")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PositionX")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PositionY")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("SkillGroup")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("St")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Stamina")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("current_timestamp");

                    b.HasKey("Id");

                    b.ToTable("players");
                });

            modelBuilder.Entity("QuantumCore.Game.Persistence.Entities.Item", b =>
                {
                    b.HasOne("QuantumCore.Game.Persistence.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("QuantumCore.Game.Persistence.Entities.PermAuth", b =>
                {
                    b.HasOne("QuantumCore.Game.Persistence.Entities.PermGroup", "Group")
                        .WithMany("Permissions")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("QuantumCore.Game.Persistence.Entities.PermUser", b =>
                {
                    b.HasOne("QuantumCore.Game.Persistence.Entities.PermGroup", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuantumCore.Game.Persistence.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("QuantumCore.Game.Persistence.Entities.PermGroup", b =>
                {
                    b.Navigation("Permissions");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
