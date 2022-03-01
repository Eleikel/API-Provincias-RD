using API_InfoProvinciasRD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Repository.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<ICollection<User>> GetUsers();
        Task<User> GetUser(int UserId);
        Task<bool> ExistUser(string user);
        Task<User> Register(User user, string password);
        Task<User> Login(string user, string password);

    }
}
