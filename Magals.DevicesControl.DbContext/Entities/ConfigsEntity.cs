using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magals.DevicesControl.DbContext.Entities;
public class ConfigsEntity : Entity
{
    [Key]
    [Required]
    public required string Name { get; set; }

    [Required]
    public required bool Enable { get; set; }

    [Required]
    public required string Type_Connect { get; set; }

    [Required]
    public required string Config { get; set; }

    [Required]
    public string ?Protocol { get; set; }

    [Required]
    public bool Autoscan { get; set; } = false;

    [Required]
    public string ?Description { get; set; }


    public SettingsDevicesEntity SettingsDevicesEntity { get; set; } = default!;

    public ICollection<CustomsettingsEntity> Customsettings { get; set; } = default!;

}
