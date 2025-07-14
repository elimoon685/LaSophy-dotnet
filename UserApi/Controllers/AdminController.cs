using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserApi.DTO;
using UserApi.Service.UserService;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        public AdminController(IUserService userService)
        {

            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {

            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password) || string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                return StatusCode(400, ApiResponse<string>.Fail("Please fill out form properly", "400"));
            }

            var result = await _userService.RegisterAdminAsync(model);
            if (result.Succeeded)
            {
                return StatusCode(200, ApiResponse<IdentityResult>.Success(result, "Register successfully"));
            }
            
                return StatusCode(500, ApiResponse<string>.Fail("Unknown error", "500"));
            
        }
    }
}
