using TaskManagementApi.DAL.Models;

namespace TaskManagementApi.BL.Interfaces
{
    public interface IUserService
    {
        Task<bool> AuthenticateUserAsync(string username, string password);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
