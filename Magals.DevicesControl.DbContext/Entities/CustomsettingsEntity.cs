using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magals.DevicesControl.DbContext.Entities;
public class CustomsettingsEntity : Entity
{
    [Required]
    public required string Key { get; init; }

    [Required]
    public required string Value { get; init; }
}
