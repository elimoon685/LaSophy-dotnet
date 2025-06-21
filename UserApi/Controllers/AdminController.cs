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
            try
            {
                var result = await _userService.RegisterAdminAsync(model);

                return StatusCode(200, ApiResponse<IdentityResult>.Success(result, "Register successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(400, ApiResponse<string>.Fail(ex.Message, "Register successfully"));
            }
        }
    }
}
