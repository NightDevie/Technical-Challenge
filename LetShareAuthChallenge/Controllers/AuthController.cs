using LetShareAuthChallenge.Dtos;
using LetShareAuthChallenge.Models;
using LetShareAuthChallenge.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace LetShareAuthChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            if (string.IsNullOrEmpty(request.Username) ||
                string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username or password cannot be empty.");
            }

            // Fetch user from DB
            User? user = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Verify password using BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate tokens
            string accessToken = GenerateJwtToken(user, true);
            string refreshToken = GenerateJwtToken(user, false);

            return Ok(new
            {
                access_token = accessToken,
                refresh_token = refreshToken
            });
        }

        private string GenerateJwtToken(User user, bool isAccessToken)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"]!;
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userId", user.UserId.ToString()),
                new Claim("username", user.Username),
                new Claim("tenantId", user.TenantId?.ToString() ?? ""),
                new Claim("role", user.Role ?? ""),
                new Claim("languageId", user.LanguageId?.ToString() ?? ""),
                new Claim("name", user.Name ?? "")
            };

            var tokenExpirationMinutes = isAccessToken
                ? int.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"]!)
                : int.Parse(_configuration["JwtSettings:RefreshTokenExpirationMinutes"]!);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

