namespace TaskManagementApi.BL.Interfaces
{
    public interface IUserService
    {
        Task<bool> AuthenticateUserAsync(string username, string password);
    }
}
