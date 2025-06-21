using System.Text.Json;
using Azure.Messaging.ServiceBus;
using CommentsApi.DTO;
using CommentsApi.DTO.ApiResponse;
using CommentsApi.Exceptions.CustomExceptions;
using CommentsApi.Models;
using CommentsApi.Services.BookCommentsServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedContract.HttpClient;

namespace CommentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private IBookCommentsService _bookCommentsService;
    

        public CommentsController(IBookCommentsService bookCommentsService)

        {
            _bookCommentsService = bookCommentsService;
        }


        [HttpGet("comments/{id}")]
        public async Task<ActionResult<CommentsDto>> GetCommentsByBookId(int id)
        {
            var bookComments = await _bookCommentsService.GetCommentsByBookId(id);
            return Ok(bookComments);
        }


        [Authorize]
        [HttpPost("commments")]

        public async Task<ActionResult<BookComments>> AddBookComments(CreateCommentsRequestDto bookComments)
        {

            var newComments = await _bookCommentsService.AddBookComments(bookComments);
            return Ok(newComments);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCommentsRequestDto bookComments)
        {
            var json = JsonSerializer.Serialize(bookComments);
            var message = new ServiceBusMessage(json)
            {
                ContentType = "application/json",

                Subject = "NewComment"

            };
            return Ok(" Comment message sent");
        }

        [HttpPost("book")]


        public async Task<ActionResult<ApiResponse<bool>>> AddBookInfo([FromBody] BookMetaData bookInfo)
        {


            
            
            
                bool result = await _bookCommentsService.AddBookInfo(bookInfo);
            if (!result)
            {
                throw new SaveBookMetaDataException("Failed to save book");
            }
                return StatusCode(200, ApiResponse<bool>.Success(result, "Book info added successfully"));

            
            
        }
    }


}
