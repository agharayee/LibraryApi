using LibraryAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public interface IAccountService
    {
        Task<LoginSucessfulDto> LoginAsync(LoginDto model);
        Task<RegistrationDto> RegisterAsync(RegisterDto model);
        Task<RegistrationDto> AddAdminToRoleAsync(string email);
    }
}
