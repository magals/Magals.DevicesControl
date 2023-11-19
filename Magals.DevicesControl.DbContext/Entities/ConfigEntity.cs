using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magals.DevicesControl.DbContext.Entities;
public class ConfigEntity : Entity
{
    [Required]
    public required string Name { get; set; }

    [Required]
    public required bool Enable { get; set; }

    [Required]
    public required string TypeConnect { get; set; }

    [Required]
    public required string Protocol { get; set; }

    [Required]
    public required bool Autoscan { get; set; }

    [Required]
    public required string Description { get; set; }


    public long CustomsettingsEntityId { get; set; }
    public List<CustomsettingsEntity> ListCustomsettingsEntity{ get; set; } = default!;

}
