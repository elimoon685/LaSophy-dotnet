using System.Security.Claims;
using CommentsApi.DTO;
using CommentsApi.DTO.ApiResponse;
using CommentsApi.DTO.Request;
using CommentsApi.Services.InteractiveServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeCollectController : ControllerBase
    {
        private readonly IInteractiveService _interactiveService;

        public LikeCollectController(IInteractiveService interactiveService)
        {

            _interactiveService = interactiveService;
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost("like")]

        public async Task<ActionResult> ToggleBookLike([FromBody] ToggleBookLikeCollectRequestDto request)

        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            //var userId=Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(400, ApiResponse<string>.Fail("User not authenticated properly", "400"));
            }

            var result = await _interactiveService.ToggleBookLikeAsync(userId, request);

            return StatusCode(200, ApiResponse<int>.Success(result, "Update like count"));

        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost("collect")]

        public async Task<ActionResult> ToggleBookCollect([FromBody] ToggleBookLikeCollectRequestDto request)

        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(400, ApiResponse<string>.Fail("User not authenticated properly", "400"));
            }

            var result = await _interactiveService.ToggleBookCollectAsync(userId, request);

            return StatusCode(200, ApiResponse<int>.Success(result, "Update like count"));

        }

        
        //[Authorize(Policy ="UserPolicy")]
        [HttpPost("like-comment")]
        public async Task <ActionResult> ToggleCommentLike([FromBody] ToggleBookCommentLikeRequestDto request)
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = "b6539b54-b93e-4a66-9d27-a361cc6ae16b";
            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(400, ApiResponse<string>.Fail("User not authenticated properly", "400"));
            }
            var result = await _interactiveService.ToggleBookCommentLikeAsync(userId, request);

            return StatusCode(200, ApiResponse<int>.Success(result, "Update commentlike count"));

        }
    }
}
