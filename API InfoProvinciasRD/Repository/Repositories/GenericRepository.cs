
using API_InfoProvinciasRD.Data;
using API_InfoProvinciasRD.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = context.Set<T>();
        }

        public virtual Task<bool> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Exist(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Exist(string name)
        {
            throw new NotImplementedException();
        }

        public virtual  Task<ICollection<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual  Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

    }
}
