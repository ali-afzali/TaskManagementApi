using Microsoft.Extensions.Configuration;
using TaskManagementApi.BL.Interfaces;
using TaskManagementApi.BL.Services;
using TaskManagementApi.DAL.Models;

namespace TaskManagementApi.BL.Tests.Services;

public class AuthenticationServiceTests
{
    [Fact]
    public async Task GenerateTokenAsync_ReturnsTokenForValidCredentials()
    {
        var configuration = BuildConfiguration();
        var userService = new StubUserService
        {
            IsAuthenticated = true,
            User = new User
            {
                Id = 3,
                Username = "admin",
                PasswordHash = "hashed-password"
            }
        };

        var service = new AuthenticationService(configuration, userService);

        var result = await service.GenerateTokenAsync("admin", "123456");

        Assert.False(string.IsNullOrWhiteSpace(result.Token));
        Assert.Equal(3, result.UserId);
        Assert.Equal("admin", result.Username);
    }

    [Fact]
    public async Task GenerateTokenAsync_ThrowsUnauthorizedAccessException_WhenAuthenticationFails()
    {
        var service = new AuthenticationService(BuildConfiguration(), new StubUserService { IsAuthenticated = false });

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.GenerateTokenAsync("admin", "wrong-password"));
    }

    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SecretKey"] = "test-secret-key-that-is-at-least-32-characters-long",
                ["Jwt:Issuer"] = "TaskManagementApi",
                ["Jwt:Audience"] = "TaskManagementApiUsers",
                ["Jwt:ExpirationMinutes"] = "60"
            })
            .Build();
    }

    private sealed class StubUserService : IUserService
    {
        public bool IsAuthenticated { get; set; }
        public User? User { get; set; }

        public Task<bool> AuthenticateUserAsync(string username, string password)
        {
            return Task.FromResult(IsAuthenticated);
        }

        public Task<User?> GetUserByUsernameAsync(string username)
        {
            return Task.FromResult(User);
        }
    }
}
