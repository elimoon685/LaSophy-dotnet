using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using NotificationsApi.DTO;

namespace NotificationsApi.Exceptions
{
    public class GlobalExceptionsHandler
    {

        public async Task HandleException(HttpContext httpContext)
        {
            var exceptionHandlingFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandlingFeature == null) { return; }

            var exceptions = exceptionHandlingFeature.Error;
            httpContext.Response.ContentType = "application/json";
            var statusCode = GetStatuscode(exceptions);
            httpContext.Response.StatusCode = statusCode;
            /*
            var errorResponse = new ErrorResponse(
                GetStatuscode(exceptions),
                GetErrorMessage(exceptions),
                exceptions.GetType().Name,
                exceptions.Message

                );
            */
            var response = ApiResponse<string>.Fail(GetErrorMessage(exceptions), GetStatuscode(exceptions).ToString());
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private int GetStatuscode(System.Exception exceptions)
        {
            if (exceptions is ArgumentNullException) { return StatusCodes.Status404NotFound; }

            else if (exceptions is KeyNotFoundException) { return StatusCodes.Status404NotFound; }

            else if (exceptions is ArgumentException ) { return StatusCodes.Status400BadRequest; }

            else { return StatusCodes.Status500InternalServerError; }
        }

        private string GetErrorMessage(Exception exceptions)
        {
            if (exceptions is ArgumentException) return "Invaild Argument";

            else if (exceptions is KeyNotFoundException) return "Value not found";

            else if (exceptions is ArgumentNullException) return "Argument can not be null";

           
            else
            {
                return "Unexpected Error";
            }


        }
    }
}
