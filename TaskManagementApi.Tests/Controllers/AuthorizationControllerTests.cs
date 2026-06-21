using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TaskManagementApi.BL.DTOs;
using TaskManagementApi.BL.Interfaces;
using TaskManagementApi.Controllers;
using TaskManagementApi.Models;

namespace TaskManagementApi.Tests.Controllers;

public class AuthorizationControllerTests
{
    [Fact]
    public async Task Login_ReturnsBadRequest_WhenCredentialsAreMissing()
    {
        var controller = new AuthorizationController(new StubAuthorizationService(), NullLogger<AuthorizationController>.Instance);

        var result = await controller.Login(new LoginRequest { Username = string.Empty, Password = string.Empty });

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequest.Value);
    }

    [Fact]
    public async Task Login_ReturnsOk_WhenCredentialsAreValid()
    {
        var expectedResponse = new LoginResponse
        {
            Token = "sample-token",
            UserId = 1,
            Username = "admin"
        };

        var controller = new AuthorizationController(
            new StubAuthorizationService(expectedResponse),
            NullLogger<AuthorizationController>.Instance);

        var result = await controller.Login(new LoginRequest { Username = "admin", Password = "123456" });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<LoginResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Token, response.Token);
        Assert.Equal(expectedResponse.UserId, response.UserId);
        Assert.Equal(expectedResponse.Username, response.Username);
    }

    private sealed class StubAuthorizationService : IAuthorizationService
    {
        private readonly LoginResponse _response;

        public StubAuthorizationService()
            : this(new LoginResponse())
        {
        }

        public StubAuthorizationService(LoginResponse response)
        {
            _response = response;
        }

        public Task<LoginResponse> GenerateTokenAsync(string username, string password)
        {
            return Task.FromResult(_response);
        }
    }
}
