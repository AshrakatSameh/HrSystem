using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamweelyHR.Application.DTOs.Auth;

namespace TamweelyHR.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);

    }

}
