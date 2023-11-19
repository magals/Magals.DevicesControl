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
    public DbSet<ConfigEntity>? ConfigEntities { get; set; }
    public DbSet<CustomsettingsEntity>? CustomsettingsEntities { get; set; }
    public DbSet<SettingsDevicesEntity>? SettingsDevicesEntities { get; set; }

    protected readonly IConfiguration Configuration;

    public SettingsDevicesDbContext(DbContextOptions<SettingsDevicesDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("settingsdevices");
    }
}
