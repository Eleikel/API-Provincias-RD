using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Models
{
    public class Region
    {
        [Key]
        public int Id { get; set; }
        public string NombreRegion {get; set;}
    }
}
