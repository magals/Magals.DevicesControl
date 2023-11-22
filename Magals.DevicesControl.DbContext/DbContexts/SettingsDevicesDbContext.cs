using Magals.DevicesControl.DbContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magals.DevicesControl.DbContext.DbContexts;
public class SettingsDevicesDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public static string NameSchema { get; set; } = "settingsdevices";
    public DbSet<ConfigsEntity>? ConfigEntities { get; set; }
    public DbSet<CustomsettingsEntity>? CustomsettingsEntities { get; set; }
    public DbSet<SettingsDevicesEntity>? SettingsDevicesEntities { get; set; }

    protected readonly IConfiguration Configuration;

    public SettingsDevicesDbContext(DbContextOptions<SettingsDevicesDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(NameSchema);

        modelBuilder.Entity<SettingsDevicesEntity>()
            .HasMany(e => e.Configs)
            .WithOne(e => e.SettingsDevicesEntity)
            .HasForeignKey(e => e.SettingsDevicesEntityName)
            .HasPrincipalKey(e => e.Name);
        
        modelBuilder.Entity<ConfigsEntity>()
            .HasMany(e => e.Customsettings)
            .WithOne(e => e.Configs)
            .HasForeignKey(e => e.ConfigsId)
            .HasPrincipalKey(e => e.Id);

        modelBuilder.Entity<CustomsettingsEntity>()
            .HasKey(e => e.Id);
    }
}
