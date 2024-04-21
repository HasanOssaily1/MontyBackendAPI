using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MontyBackendAPI.Controllers;
using MontyBackendAPI.Models;

namespace MontyBackendAPI.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly MyContext _context;

        public UsersRepository(MyContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                // User not found, handle accordingly
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            return await _context.Users.OrderBy(x => x.id).ToListAsync();
        }

        public async Task<Users> GetUserById(int id)
        {

            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> Create(Users user)
        {
            var exists = await _context.Users.FirstOrDefaultAsync(x => x.email == user.email);
            if (exists == null)
            {
                _context.Add(user);
                await _context.SaveChangesAsync(); // Save changes to the database
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public async Task Update(Users user)
        {
            var entry = _context.Entry(user);
            entry.State = EntityState.Modified;
            if(user.password != "")
            {
                user.password = AuthController.HashPassword(user.password);
            }
            else
            {
                entry.Property(x => x.password).IsModified = false;
            }
            await _context.SaveChangesAsync(); // Save changes to the database
        }

        
        public async Task<Users> GetUserByEmail(string Email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.email == Email);
        }
    }

}
