using TaskManagementApi.BL.DTOs;

namespace TaskManagementApi.BL.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> GenerateTokenAsync(string username, string password);
    }
}
