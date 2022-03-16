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
    public class RegionRepository : GenericRepository<Region>, IRegionRepository
    {
        public RegionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<bool> Exist(string regionName)
        {
            bool exist = await dbSet.AnyAsync(a => a.NombreRegion.ToLower().Trim() == regionName.ToLower().Trim());
            return exist;
        }

        public override async Task<bool> Exist(int id)
        {
            bool exist = await dbSet.AnyAsync(a => a.Id == id);
            return exist;
        }

        public override async Task<ICollection<Region>> GetAll()
        {
            return await dbSet.OrderBy(a => a.Id).ToListAsync();

        }
        public override async Task<Region> GetById(int regionId)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.Id == regionId);
        }
        public async override Task<bool> Add(Region entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public override Task<bool> Update(Region entity)
        {
            dbSet.Update(entity);
            return Task.FromResult(true);
        }

        public override Task<bool> Delete(Region entity)
        {
            //poner async
            dbSet.Remove(entity);
            return Task.FromResult(true);
        }
    }
}
