using Library.Data.Entities;
using LibraryAPI.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._configuration = configuration;
        }
        public async Task<LoginSucessfulDto> LoginAsync(LoginDto model)
        {
            JwtSecurityToken token = default;
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSiginKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
                token = new JwtSecurityToken(
                   issuer: _configuration["JWT:ValidIssuer"],
                   audience: _configuration["JWT:ValidAudience"],
                   expires: DateTime.Now.AddHours(2),
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSiginKey, SecurityAlgorithms.HmacSha256Signature)
                   ); ;
            }
            else
            {
                return null;
            }

            var loginDto = new LoginSucessfulDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo.ToString("yyyy-MM-ddThh:mm:ss"),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return loginDto;

        }

        public async Task<RegistrationDto> RegisterAsync(RegisterDto model)
        {
            RegistrationDto returnDto = default;
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MatriculationNumber = model.MatriculationNumber,
                NationalIDCardNumber = model.MatriculationNumber,
                PhoneNumber = model.PhoneNumber
            };
            returnDto = new RegistrationDto
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber

            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, LibraryAPI.Authorization.Autheorization.default_role.ToString());
                    var roles = await _userManager.GetRolesAsync(user);
                    returnDto.UserRoles = (List<string>)roles;
                    return returnDto;
                }
                else
                {
                    var errors = AddErrors(result);
                    returnDto.ErrorMessage = errors;
                    return returnDto;
                }

            }
            else
            {
                var error = $"Email {user.Email } is already registered.";
                returnDto.ErrorMessage = error;
                return returnDto;
            }
        }

        public async Task<RegistrationDto> AddAdminToRoleAsync(string email)
        {
            if(email == null)
            {
                var error= new RegistrationDto
                {
                    ErrorMessage = "Email cannot be null"
                };
                return error;
            }
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                var noUserFound = new RegistrationDto
                {
                    ErrorMessage = "Email cannot be null"
                };
                return noUserFound;
            }
            await _userManager.AddToRoleAsync(user, "Administrator");
            var roles = await _userManager.GetRolesAsync(user);
            var addedSuccessfully = new RegistrationDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserRoles = (List<string>)roles
            };
            return addedSuccessfully;
        }
        private string AddErrors(IdentityResult result)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var error in result.Errors)
            {
                sb.Append(error.Description + " Registration Failed. ");
            }
            return sb.ToString();
        }


    }
}
