using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Models.Dto
{
    public class UserDto
    {
        public int Id { get; set; }       
        public string UserA { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}
