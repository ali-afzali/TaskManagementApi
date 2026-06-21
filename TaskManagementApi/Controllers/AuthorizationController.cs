using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskManagementApi.Models;
using TaskManagementApi.BL.DTOs;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly BL.Interfaces.IAuthorizationService _authorizationService;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(BL.Interfaces.IAuthorizationService authorizationService, ILogger<AuthorizationController> logger)
        {
            _authorizationService = authorizationService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<TaskManagementApi.BL.DTOs.LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request?.Username) || string.IsNullOrWhiteSpace(request?.Password))
                {
                    return BadRequest(new { message = "Username and password are required" });
                }

                var loginResponse = await _authorizationService.GenerateTokenAsync(request.Username, request.Password);
                _logger.LogInformation("Token generated successfully for user: {Username}, UserId: {UserId}", request.Username, loginResponse.UserId);

                return Ok(loginResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Invalid credentials for user: {Username}", request?.Username);
                return Unauthorized(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid login request");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating authorization token");
                return StatusCode(500, new { message = "An error occurred while generating the token" });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            try
            {
                var username = User.Identity?.Name;
                _logger.LogInformation("User logged out successfully: {Username}", username);
                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(500, new { message = "An error occurred while logging out" });
            }
        }


    }
}
