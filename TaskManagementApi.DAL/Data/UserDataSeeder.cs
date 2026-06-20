using TaskManagementApi.DAL.Models;

namespace TaskManagementApi.DAL.Data
{
    public static class UserDataSeeder
    {
        public static List<User> GetSeedData()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                    CreatedDate = DateTime.UtcNow
                }
            };
        }
    }
}
