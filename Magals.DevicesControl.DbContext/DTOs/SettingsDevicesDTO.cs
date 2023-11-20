using Magals.DevicesControl.DbContext.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magals.DevicesControl.DbContext.DTOs;
public class SettingsDevicesDTO
{
    public required string Name { get; set; }

    public required bool Enable { get; set; }

    public ICollection<ConfigsDTO> Configs { get; set; } = default!;
}
