﻿// <auto-generated />
using System;
using Magals.DevicesControl.DbContext.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApplicationGenMigrations.Migrations
{
    [DbContext(typeof(SettingsDevicesDbContext))]
    [Migration("20231119163444_InitialMigrations")]
    partial class InitialMigrations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("settingsdevices")
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Magals.DevicesControl.DbContext.Entities.ConfigEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("Autoscan")
                        .HasColumnType("boolean");

                    b.Property<long>("CustomsettingsEntityId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Enable")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Protocol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("SettingsDevicesEntityId")
                        .HasColumnType("bigint");

                    b.Property<string>("TypeConnect")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SettingsDevicesEntityId");

                    b.ToTable("ConfigEntities", "settingsdevices");
                });

            modelBuilder.Entity("Magals.DevicesControl.DbContext.Entities.CustomsettingsEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long?>("ConfigEntityId")
                        .HasColumnType("bigint");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ConfigEntityId");

                    b.ToTable("CustomsettingsEntities", "settingsdevices");
                });

            modelBuilder.Entity("Magals.DevicesControl.DbContext.Entities.SettingsDevicesEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("ConfigEntityId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Enable")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SettingsDevicesEntities", "settingsdevices");
                });

            modelBuilder.Entity("Magals.DevicesControl.DbContext.Entities.ConfigEntity", b =>
                {
                    b.HasOne("Magals.DevicesControl.DbContext.Entities.SettingsDevicesEntity", null)
                        .WithMany("ListConfigEntity")
                        .HasForeignKey("SettingsDevicesEntityId");
                });

            modelBuilder.Entity("Magals.DevicesControl.DbContext.Entities.CustomsettingsEntity", b =>
                {
                    b.HasOne("Magals.DevicesControl.DbContext.Entities.ConfigEntity", null)
                        .WithMany("ListCustomsettingsEntity")
                        .HasForeignKey("ConfigEntityId");
                });

            modelBuilder.Entity("Magals.DevicesControl.DbContext.Entities.ConfigEntity", b =>
                {
                    b.Navigation("ListCustomsettingsEntity");
                });

            modelBuilder.Entity("Magals.DevicesControl.DbContext.Entities.SettingsDevicesEntity", b =>
                {
                    b.Navigation("ListConfigEntity");
                });
#pragma warning restore 612, 618
        }
    }
}
