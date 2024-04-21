using MontyBackendAPI.Models;

namespace MontyBackendAPI.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<Users>> GetAllUsers();
        Task<Users> GetUserById(int Id);

        Task<Users> GetUserByEmail(string Email);
        Task<bool> Create(Users user);
        Task Update(Users user);
        Task<bool> Delete(int Id);
    }
}
