using Magals.DevicesControl.DbContext.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Magals.DevicesControl.DbContext.DTOs;
public class ConfigsDTO
{
    public required string name { get; set; }

    public required bool enable { get; set; }

    public required string type_connect { get; set; }

    public required JsonElement config { get; set; }

    public required string protocol { get; set; }

    public required bool autoscan { get; set; }

    public required string description { get; set; }

    public ICollection<CustomsettingsDTO> customsettings { get; set; } = default!;
}
