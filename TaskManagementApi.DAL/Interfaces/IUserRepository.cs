using TaskManagementApi.DAL.Models;

namespace TaskManagementApi.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
    }
}
