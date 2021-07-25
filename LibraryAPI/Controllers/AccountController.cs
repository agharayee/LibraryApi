using LibraryAPI.Dtos;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            this._accountService = accountService;
            this._logger = logger;
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
                    _logger.LogInformation($"{result.Email} accessed the Register EndPoint on {DateTime.Now}");
                    return BadRequest(result.ErrorMessage);
                }
                else
                {
                    _logger.LogInformation($"{result.Email} accessed the Register EndPoint on {DateTime.Now}");
                    return Ok(result);
                }
                   
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null) return BadRequest();
            var result = await _accountService.LoginAsync(loginDto);
            if (result == null)
            {
                _logger.LogInformation($"{result.Email} accessed the Login EndPoint on {DateTime.Now}");
                return BadRequest("Invalid Credentials");
            }

            else
            {
                _logger.LogInformation($"{result.Email} accessed the Login EndPoint on {DateTime.Now}");
                return Ok(result);
            }
        }

        [HttpPost("AddToAdminRole")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddUserToAdminRoleAsync([FromBody] string email)
        {
            var result = await _accountService.AddAdminToRoleAsync(email);
            _logger.LogInformation($"{result.Email} accessed the AddUserToAdminRoleAsync EndPoint on {DateTime.Now}");
            return Ok(result);
        }
    }
}
