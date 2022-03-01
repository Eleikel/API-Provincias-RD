using API_InfoProvinciasRD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Repository.IRepositories
{
    public interface IProvinciaRepository : IGenericRepository<Provincia>
    {
        Task<IEnumerable<Provincia>> Search(string name);

    }
}
