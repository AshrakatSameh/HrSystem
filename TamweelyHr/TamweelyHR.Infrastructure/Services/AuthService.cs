using Microsoft.AspNetCore.Identity;  // ← ADD THIS for UserManager
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TamweelyHr.Domain.Entities;
using TamweelyHR.Application.DTOs.Auth;
using TamweelyHR.Application.Interfaces;

namespace TamweelyHR.Infrastructure.Services
{
    /// <summary>
    /// Authentication service using JWT tokens.
    /// Implements Bearer token authentication.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;  // ← Added generic type
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,  // ← Added generic type
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Authenticates user and generates JWT token.
        /// </summary>
        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            // Find user by username
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Verify password
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Generate JWT token
            var token = GenerateJwtToken(user, roles.ToList());

            return new LoginResponseDto
            {
                Token = token,
                UserName = user.UserName!,
                FullName = user.FullName,
                Roles = roles.ToList()
            };
        }

        /// <summary>
        /// Generates a JWT token with user claims.
        /// Token is valid for 7 days.
        /// </summary>
        private string GenerateJwtToken(ApplicationUser user, List<string> roles)
        {
            // Create claims - information embedded in the token
            var claims = new List<Claim>  // ← Fixed: System.Security.Claims.Claim
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Get secret key from configuration
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            // Create signing credentials
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            // Create token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            // Return serialized token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}