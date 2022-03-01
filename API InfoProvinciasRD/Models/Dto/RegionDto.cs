using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Models.Dto
{
    public class RegionDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre Region es obligatorio.")]
        public string NombreRegion { get; set; }


    }
}
