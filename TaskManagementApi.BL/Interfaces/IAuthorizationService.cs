using TaskManagementApi.BL.DTOs;

namespace TaskManagementApi.BL.Interfaces
{
    public interface IAuthorizationService
    {
        Task<LoginResponse> GenerateTokenAsync(string username, string password);
    }
}
