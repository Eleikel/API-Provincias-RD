using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Models
{
    public class Provincia
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Fundacion { get; set; }
        public string Superficie { get; set; }

        public int regionId { get; set; }
        [ForeignKey("regionId")]
        public Region Region { get; set; }
    }
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
}
