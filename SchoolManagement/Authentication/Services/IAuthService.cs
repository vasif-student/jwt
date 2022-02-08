using Authentication.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginModelDto login);
    }
}
