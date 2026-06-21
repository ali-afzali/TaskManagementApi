using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskManagementApi.BL.Interfaces;
using TaskManagementApi.BL.DTOs;

namespace TaskManagementApi.BL.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationMinutes;

        public AuthorizationService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
            _secretKey = _configuration["Jwt:SecretKey"] ?? "your-super-secret-key-that-is-at-least-32-characters-long!!!";
            _issuer = _configuration["Jwt:Issuer"] ?? "TaskManagementApi";
            _audience = _configuration["Jwt:Audience"] ?? "TaskManagementApiUsers";
            _expirationMinutes = int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var minutes) ? minutes : 60;
        }

        public async Task<LoginResponse> GenerateTokenAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username and password cannot be empty");
            }

            // Authenticate user
            var isAuthenticated = await _userService.AuthenticateUserAsync(username, password);
            if (!isAuthenticated)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Get user details
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, username),
                new Claim("userId", user.Id.ToString()),
                new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expirationMinutes),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return await Task.FromResult(new LoginResponse
            {
                Token = tokenString,
                UserId = user.Id,
                Username = user.Username
            });
        }
    }
}
