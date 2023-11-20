using Magals.DevicesControl.DbContext.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magals.DevicesControl.DbContext.DTOs;
public class ConfigsDTO
{
    public required string Name { get; set; }

    public required bool Enable { get; set; }

    public required string Type_Connect { get; set; }

    public required string Config { get; set; }

    public required string Protocol { get; set; }

    public required bool Autoscan { get; set; }

    public required string Description { get; set; }

    public ICollection<CustomsettingsDTO> Customsettings { get; set; } = default!;
}
