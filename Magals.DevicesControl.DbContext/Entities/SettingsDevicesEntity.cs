using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magals.DevicesControl.DbContext.Entities;
public class SettingsDevicesEntity : Entity
{
    [Required]
    public required string Name { get; set; }

    [Required]
    public required bool Enable { get; set; }

    public long ConfigEntityId { get; set; }
    public List<ConfigEntity> ListConfigEntity { get; set; } = default!;
}
