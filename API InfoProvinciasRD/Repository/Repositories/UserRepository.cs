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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {


        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistUser(string user)
        {
            if (await dbSet.AnyAsync(x => x.UserA == user))
            {
                return true;
            }
            return false;
        }

        public Task<User> GetUser(int UserId)
        {
            return dbSet.FirstOrDefaultAsync(x => x.Id == UserId);
        }

        public async Task<ICollection<User>> GetUsers()
        {
            return await dbSet.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<User> Login(string user, string password)
        {

            var getUser = dbSet.FirstOrDefault(x => x.UserA == user);

            if (getUser == null)
            {
                return null;
            }
            if (!await VerifyPasswordHash(password, getUser.PasswordHash, getUser.PasswordSalt))
            {
                return null;
            }
            return getUser;
        }


        public Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            dbSet.Add(user);
            return Task.FromResult(user); ;
        }




        private Task<bool> VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var hashComputado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < hashComputado.Length; i++)
                {
                    if (hashComputado[i] != passwordHash[i])
                    {
                        return Task.FromResult(false);
                    }
                }
            }
            return Task.FromResult(true);
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
