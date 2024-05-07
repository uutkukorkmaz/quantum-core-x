﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuantumCore.Auth.Persistence;

#nullable disable

namespace QuantumCore.Auth.Persistence.Migrations.Mysql
{
    [DbContext(typeof(MySqlAuthDbContext))]
    partial class MySqlAuthDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("QuantumCore.Auth.Persistence.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("(CAST(CURRENT_TIMESTAMP AS DATETIME(6)))");

                    b.Property<string>("DeleteCode")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("varchar(7)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("varchar(60)");

                    b.Property<short>("Status")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("(CAST(CURRENT_TIMESTAMP AS DATETIME(6)))");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("Status")
                        .IsUnique();

                    b.ToTable("accounts");
                });

            modelBuilder.Entity("QuantumCore.Auth.Persistence.Entities.AccountStatus", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<short>("Id"));

                    b.Property<bool>("AllowLogin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ClientStatus")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("varchar(8)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("account_status");
                });

            modelBuilder.Entity("QuantumCore.Auth.Persistence.Entities.Account", b =>
                {
                    b.HasOne("QuantumCore.Auth.Persistence.Entities.AccountStatus", "AccountStatus")
                        .WithOne("Account")
                        .HasForeignKey("QuantumCore.Auth.Persistence.Entities.Account", "Status")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountStatus");
                });

            modelBuilder.Entity("QuantumCore.Auth.Persistence.Entities.AccountStatus", b =>
                {
                    b.Navigation("Account")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
