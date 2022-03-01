using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Models.Dto
{
    public class ProvinciaDto
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Descripcion es obligatorio")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo Fundacion es obligatorio")]
        public string Fundacion { get; set; }
        [Required(ErrorMessage = "El campo Superficie es obligatorio")]
        public string Superficie { get; set; }
        //Foreign key
        public int RegionId { get; set; }
        public Region Region { get; set; }

    }
}
