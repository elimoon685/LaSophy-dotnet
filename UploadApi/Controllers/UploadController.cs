using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using UploadApi.DTO;
using UploadApi.DTO.ApiResponse;
using UploadApi.Service.AzureBlobStorage;

namespace UploadApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IBookUploadService _bookUploadService;
        public UploadController( IBookUploadService bookUploadService) 
        {
            _bookUploadService = bookUploadService;
        }
        //[Authorize(Policy = "AdminPolicy")]
        [ProducesResponseType(typeof(ApiResponse<UploadResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadContentRequest formData)
        {
            
           
                UploadResponse uploadPdf = await _bookUploadService.UploadPDFAsync(formData, "bookpdf","bookcover");

                return StatusCode(200, ApiResponse<UploadResponse>.Success(uploadPdf, "Save the file and metaData successfully"));
            


        }
    }
}
