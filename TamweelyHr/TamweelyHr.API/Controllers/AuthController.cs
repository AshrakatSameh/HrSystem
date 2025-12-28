using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TamweelyHR.Application.DTOs.Auth;
using TamweelyHR.Application.Interfaces;

namespace TamweelyHr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// 
        /// Login endpoint - generates JWT token.
        /// 
        /// Username and password
        /// JWT token and user information
        /// Login successful
        /// Invalid credentials
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            return Ok(result);
        }
    }
}
