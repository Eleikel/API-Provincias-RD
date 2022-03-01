using API_InfoProvinciasRD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Region> Region { get; set; }
        public DbSet<Provincia> Provincia { get; set; }
        public DbSet<User> User { get; set; }
    }
}
