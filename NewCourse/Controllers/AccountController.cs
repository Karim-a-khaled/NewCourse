using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewCourse.Entities.DTOs;
using NewCourse.Entities.Entities;
using NewCourse.Services;

namespace NewCourse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet, Authorize]
        public ActionResult<string> GetMe()
        {
            return Ok("Authorized");
        }

        [HttpPost("Register")]
        public async Task<string> Register(UserDto userDto)
        {
            var result = await _accountService.Register(userDto);

            if (result is null)
                return "This Email Is Taken";

            return "Registered Succesfuly";
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var result = await _accountService.Login(request);

            if (result == null)
                return "Invalid Email Or Password";

            return result;
        }

        [HttpPost("logout")]
        public async Task<ActionResult<string>> Logout()
        {
            var user = await _accountService.LogOut();
            if (user == null)
                return BadRequest("Unable");
            return Ok("Successfully logged out.");

        }



    }
}
