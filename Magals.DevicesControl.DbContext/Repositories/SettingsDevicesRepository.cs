using Magals.DevicesControl.DbContext.DbContexts;
using Magals.DevicesControl.DbContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magals.DevicesControl.DbContext.Repositories;
public class SettingsDevicesRepository
{
    private readonly ILogger<SettingsDevicesRepository> logger;
    private readonly SettingsDevicesDbContext settingsDevicesDbContext;

    public SettingsDevicesRepository(ILoggerFactory logger, SettingsDevicesDbContext settingsDevicesDbContext)
    {
        this.logger = logger.CreateLogger<SettingsDevicesRepository>();
        this.settingsDevicesDbContext = settingsDevicesDbContext;
    }

    public async Task Add(SettingsDevicesEntity settingsDevicesEntity)
    {
        ArgumentNullException.ThrowIfNull(settingsDevicesDbContext.SettingsDevicesEntities);

        settingsDevicesDbContext.SettingsDevicesEntities.Add(settingsDevicesEntity);
        await settingsDevicesDbContext.SaveChangesAsync();
    }

    public async Task<ICollection<SettingsDevicesEntity>> GetAllSettings()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(settingsDevicesDbContext.SettingsDevicesEntities);
            return await settingsDevicesDbContext.SettingsDevicesEntities.Include(x => x.Configs).ThenInclude(y => y.Customsettings).ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex.ToString());
            throw;
        }
    }

    public async Task<ICollection<SettingsDevicesEntity>> GetAllSettings(string namedevice)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(settingsDevicesDbContext.SettingsDevicesEntities);
            return await settingsDevicesDbContext.SettingsDevicesEntities.Where(x => string.Equals(x.Name, namedevice)).ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex.ToString());
            throw;
        }
    }

    public async Task AddConfig(string namedevice, ConfigsEntity configEntity)
    {
        ArgumentNullException.ThrowIfNull(settingsDevicesDbContext.SettingsDevicesEntities);
        ArgumentNullException.ThrowIfNull(settingsDevicesDbContext.ConfigEntities);

        var sd = settingsDevicesDbContext.SettingsDevicesEntities.First(x => string.Equals(x.Name, namedevice));

        configEntity.SettingsDevicesEntity = sd;

        settingsDevicesDbContext.ConfigEntities.Add(configEntity);
        await settingsDevicesDbContext.SaveChangesAsync();
    }

    public void AddCustomsetting(string namedevice, string nameconfig, CustomsettingsEntity customsettingsEntity)
    {
        ArgumentNullException.ThrowIfNull(settingsDevicesDbContext.CustomsettingsEntities);

        settingsDevicesDbContext.CustomsettingsEntities.Add(customsettingsEntity);
        settingsDevicesDbContext.SaveChanges();
    }

    public async Task<ICollection<CustomsettingsEntity>> GetAllCustomsettings()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(settingsDevicesDbContext.CustomsettingsEntities);
            return await settingsDevicesDbContext.CustomsettingsEntities.ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex.ToString());
            throw;
        }
    }
}
