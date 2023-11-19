using Magals.DevicesControl.DbContext.DbContexts;
using Magals.DevicesControl.DbContext.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magals.DevicesControl.DbContext.Repositories;
public class ConfigRepository
{
    private readonly ILogger<ConfigRepository> logger;
    private readonly SettingsDevicesDbContext settingsDevicesDbContext;
    public ConfigRepository(ILoggerFactory logger, SettingsDevicesDbContext settingsDevicesDbContext)
    {
        this.logger = logger.CreateLogger<ConfigRepository>();
        this.settingsDevicesDbContext = settingsDevicesDbContext;
    }

    public async Task Add(string namedevice, ConfigEntity configEntity)
    {
        ArgumentNullException.ThrowIfNull(settingsDevicesDbContext.ConfigEntities);

        settingsDevicesDbContext.ConfigEntities.Add(configEntity);
        await settingsDevicesDbContext.SaveChangesAsync();
    }
}
