using API_InfoProvinciasRD.Data;
using API_InfoProvinciasRD.Models;
using API_InfoProvinciasRD.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Repository.Repositories
{
    public class ProvinciaRepository : GenericRepository<Provincia>, IProvinciaRepository
    {
        public ProvinciaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<bool> Exist(int id)
        {
            var exist = await dbSet.AnyAsync(x => x.Id == id);
            return exist;
        }

        public override async Task<bool> Exist(string nombreProvincia)
        {
            var exist = await dbSet.AnyAsync(x => x.Nombre.ToLower().Trim() == nombreProvincia.ToLower().Trim());
            return exist;
        }

        public override async Task<ICollection<Provincia>> GetAll()
        {
            return await dbSet.Include(region => region.Region).OrderBy(a => a.Nombre).ToListAsync();
        }

        public override async Task<Provincia> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public override async Task<bool> Add(Provincia provincia)
        {
            await dbSet.AddAsync(provincia);
            return true;            
        }

        public override Task<bool> Update(Provincia entity)
        {
            dbSet.Update(entity);
            return Task.FromResult(true);
        }

        public override Task<bool> Delete(Provincia entity)
        {
            dbSet.Remove(entity);
            return Task.FromResult(true);
        }

        public  async Task<IEnumerable<Provincia>> Search(string name)
        {
            IQueryable<Provincia> query =  dbSet;

            if ( !string.IsNullOrEmpty(name))
            {
                query =  query.Where(e => e.Nombre.Contains(name) || e.Descripcion.Contains(name));
            }

            return await query.ToListAsync();
        }
    }
}
