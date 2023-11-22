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
    public required string name { get; set; }

    public required bool enable { get; set; }

    public ICollection<ConfigsDTO> configs { get; set; } = default!;
}
