using System.Threading.Tasks;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace SampleJTWAuthNoMiddleware.Controllers
{
    [ApiController]
    [Route("v1")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticateRequest model)
        {
            var response = await _userService.AuthenticateAsync(model);

            if (response == null)
                return BadRequest(new { message = "UserName or Password is incorrect." });

            return Ok(response);
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("dynamic")]
        public async Task<IActionResult> GetDynamicAsync()
        {
            var data = await _userService.GetDynamicAsync<dynamic>(string.Empty);
            return Ok(data);
        }

    }
}
