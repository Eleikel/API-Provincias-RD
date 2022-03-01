using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Models
{
    public class UserAuthDto
    {

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string User { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "La contraseña debe estar entre 4 y 10 caracteres")]
        public string Password { get; set; }


    }
}
