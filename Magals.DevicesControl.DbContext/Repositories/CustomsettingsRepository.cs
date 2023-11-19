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
public class CustomsettingsRepository
{
    private readonly ILogger<CustomsettingsRepository> logger;
    private readonly SettingsDevicesDbContext settingsDevicesDbContext;

    public CustomsettingsRepository(ILoggerFactory logger, SettingsDevicesDbContext settingsDevicesDbContext)
    {
        this.logger = logger.CreateLogger<CustomsettingsRepository>();
        this.settingsDevicesDbContext = settingsDevicesDbContext;
    }

    public void Add(string namedevice, string nameconfig, CustomsettingsEntity customsettingsEntity)
    {
        ArgumentNullException.ThrowIfNull(settingsDevicesDbContext.CustomsettingsEntities);

        settingsDevicesDbContext.CustomsettingsEntities.Add(customsettingsEntity);
        settingsDevicesDbContext.SaveChanges();
    }

    public async Task<ICollection<CustomsettingsEntity>> GetAllSettings()
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
