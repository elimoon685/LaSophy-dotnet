using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using UserApi.DTO;
using UserApi.Models;
using UserApi.Service.UserService;
using UserApi.Utils;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private UserManager<User> _userManager;
        private IConfiguration _configuration;
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
                
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginRequestDto>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            string? token=await _userService.AuthenticateUserAsync(loginRequestDto);

            return Ok(ApiResponse<string?>.Success(token,"create the token successfully"));
          
           
            
        }

        [HttpPost("forgetPassword")]
        public async Task<ActionResult> SendEmail([FromBody] string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result=await SendEmail(email);
            return Ok(result);
        }
       
    }
}
