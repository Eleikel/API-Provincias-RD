using API_InfoProvinciasRD.Models;
using API_InfoProvinciasRD.Repository.IConfiguration;
using API_InfoProvinciasRD.Repository.IRepositories;
using API_InfoProvinciasRD.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        public IRegionRepository Region { get; private set; }
        public IProvinciaRepository Provincia { get; private set; }
        public IUserRepository User { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Region = new RegionRepository(context);
            Provincia = new ProvinciaRepository(context);
            User = new UserRepository(context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
