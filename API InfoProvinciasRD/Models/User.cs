using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserA { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
