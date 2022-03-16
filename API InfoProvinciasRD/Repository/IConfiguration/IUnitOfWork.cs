using API_InfoProvinciasRD.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Repository.IConfiguration
{
    public interface IUnitOfWork
    {
        IRegionRepository Region { get; }
        IProvinciaRepository Provincia { get; }
        IUserRepository User { get; }

        Task CompleteAsync();
    }
}
