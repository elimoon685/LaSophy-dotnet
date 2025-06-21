using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using UploadApi.DTO.ApiResponse;

namespace UploadApi.Extension
{
    public class UseExceptionHandler
    {
        public static async Task HandleAsync(StatusCodeContext context)
        {
            var response = context.HttpContext.Response;

            if (response.StatusCode == StatusCodes.Status403Forbidden)
            {
                response.ContentType = "application/json";
                var error = ApiResponse<string>.Fail("Forbidden", "403");
                await response.WriteAsync(JsonSerializer.Serialize(error));
            }

            else if(response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                response.ContentType = "application/json";
                var error = ApiResponse<string>.Fail("Unauthorized", "401");
                await response.WriteAsync(JsonSerializer.Serialize(error));
            }
        }
    }
}
