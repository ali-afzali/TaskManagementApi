namespace TaskManagementApi.BL.Interfaces
{
    public interface IAuthorizationService
    {
        Task<string> GenerateTokenAsync(string username, string password);
    }
}
