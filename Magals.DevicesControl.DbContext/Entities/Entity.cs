using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magals.DevicesControl.DbContext.Entities;
public class Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(Order = 0)]
    public long Id { get; set; }
}
