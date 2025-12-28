using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamweelyHR.Application.DTOs.Auth
{
    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    //login reponse dto
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } =new();
    }
}
