using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magals.DevicesControl.DbContext.DTOs;
public class CustomsettingsDTO
{
    public required string key { get; init; }

    public required string value { get; init; }
}
