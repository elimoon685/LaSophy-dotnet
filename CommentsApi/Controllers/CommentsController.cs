using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using CommentsApi.DTO;
using CommentsApi.DTO.ApiResponse;
using CommentsApi.DTO.Request;
using CommentsApi.DTO.Response;
using CommentsApi.Exceptions.CustomExceptions;
using CommentsApi.Models;
using CommentsApi.Services.BookCommentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedContract.HttpClient;

namespace CommentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private IBookCommentService _bookCommentService;
        private ILogger<CommentsController> _logger;

        public CommentsController(IBookCommentService bookCommentService, ILogger<CommentsController> logger)

        {
            _bookCommentService = bookCommentService;
            _logger = logger;
        }


        [HttpGet("get-comments/{id}")]
        [ProducesResponseType(typeof(ApiResponse<List<GetCommentsResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCommentsByBookId(int id)
        {
            var bookComments = await _bookCommentService.GetCommentsByBookId(id);
            return StatusCode(200, ApiResponse<List<GetCommentsResponseDto>>.Success(bookComments, "get all comments successfully"));
        }


        //[Authorize(Policy = "UserPolicy")]
        [HttpPost("comments")]
        [ProducesResponseType(typeof(ApiResponse<GetCommentsResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookComments>> AddBookComments(CreateCommentsRequestDto bookComments)
        {
            /*
            foreach (var claim in User.Claims)
            {
                _logger.LogInformation($"CLAIM: {claim.Type} => {claim.Value}");
            }
            _logger.LogInformation("== END CLAIM DUMP ==");
            */

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var username = User.FindFirstValue(ClaimTypes.Name);

            var userId = "b6539b54-b93e-4a66-9d27-a361cc6ae16b";
            var username = "Elinor";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userId))
            {
                return StatusCode(400, ApiResponse<string>.Fail("User not authenticated properly", "400"));
            }

            var addedResult = await _bookCommentService.AddBookComments(bookComments,username, userId);
            
                return StatusCode(200, ApiResponse<GetCommentsResponseDto>.Success(addedResult, "Comment successful"));

            
         
            
        }

        [HttpPost("book")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult> AddBookInfo([FromBody] BookMetaData bookInfo)
        { 
            
                bool result = await _bookCommentService.AddBookInfo(bookInfo);
                return StatusCode(200, ApiResponse<bool>.Success(result, "Book info added successfully"));

            
            
        }
        [HttpGet("book")]
        [ProducesResponseType(typeof(ApiResponse<List<GetAllBooksInfoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllBooksInfo()
        {
            List<GetAllBooksInfoResponseDto>  results = await _bookCommentService.GetAllBooksInfo();

            return StatusCode(200, ApiResponse<List<GetAllBooksInfoResponseDto>>.Success(results, "Get all books info successfully"));
        }


        [HttpGet("book/{pdfPath}")]

        [ProducesResponseType(typeof(ApiResponse<GetBookInfoResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult>  GetBooksByPdfPath(string pdfPath)
        {
             var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = "d689f28c-e4f3-4a5f-b6fa-121a36b8dc0e";

            GetBookInfoResponseDto result =await _bookCommentService.GetBookInfoByPdfPath(pdfPath, userId);

            return StatusCode(200, ApiResponse<GetBookInfoResponseDto>.Success(result, "Get book info successfully"));

        }
        //[Authorize(Policy = "UserPolicy")]
        [HttpDelete("delete-comment/{commentId}")]

        public async Task<ActionResult> DeleteCommentById(int commentId)
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var deleteResult=await _bookCommentService.DeleteCommentByIdAsync(commentId);

            return StatusCode(200, ApiResponse<bool>.Success(deleteResult, "Delete your comment successfully"));

        }


    }   



}
