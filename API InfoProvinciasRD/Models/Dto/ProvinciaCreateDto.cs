using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Models.Dto
{
    public class ProvinciaCreateDto
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

        public int regionId { get; set; }
        
    }

}

