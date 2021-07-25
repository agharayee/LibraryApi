using LibraryAPI.Dtos;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("Account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            if (register == null) return BadRequest();
            else
            {
                var result = await _accountService.RegisterAsync(register);
                if (result.ErrorMessage != null)
                {
                    return BadRequest(result.ErrorMessage);
                }
                else
                    return Ok(result);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null) return BadRequest();
            var result = await _accountService.LoginAsync(loginDto);
            if (result == null)
            {
                return BadRequest("Invalid Credentials");
            }

            else
            {
                return Ok(result);
            }
        }

        [HttpPost("AddToAdminRole")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddUserToAdminRoleAsync([FromBody] string email)
        {
            var result = await _accountService.AddAdminToRoleAsync(email);
            return Ok(result);
        }
    }
}
