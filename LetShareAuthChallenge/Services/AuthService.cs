using LetShareAuthChallenge.Models;
using LetShareAuthChallenge.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LetShareAuthChallenge.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<(string AccessToken, string RefreshToken)> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Username or password cannot be empty.");
            }

            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            var access = GenerateJwt(user, true);
            var refresh = GenerateJwt(user, false);

            return (access, refresh);
        }

        private string GenerateJwt(User user, bool isAccess)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"]!;
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userId", user.UserId),
                new Claim("username", user.Username),
                new Claim("role", user.Role ?? ""),
                new Claim("tenantId", user.TenantId ?? ""),
                new Claim("languageId", user.LanguageId ?? ""),
                new Claim("firstName", user.FirstName ?? ""),
                new Claim("lastName", user.LastName ?? "")
            };
            

            var expiration = isAccess
                ? int.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"]!)
                : int.Parse(_configuration["JwtSettings:RefreshTokenExpirationMinutes"]!);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiration),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
