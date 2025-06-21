using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using UserApi.DTO;
using UserApi.Exceptions.CustomExceptions;

namespace UserApi.Exceptions
{
    public class GlobalExceptionHandler
    {
        public async Task HandleException(HttpContext httpContext)
        {
            var exceptionHandlingFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandlingFeature == null) { return; }

            var exceptions = exceptionHandlingFeature.Error;
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = GetStatuscode(exceptions);

            var errorResponse = new ErrorResponse(
                GetStatuscode(exceptions),
                GetErrorMessage(exceptions),
                exceptions.GetType().Name,
                exceptions.Message
                );

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        private int GetStatuscode(System.Exception exceptions)
        {
            if (exceptions is ArgumentNullException) { return StatusCodes.Status404NotFound; }

            else if (exceptions is KeyNotFoundException) { return StatusCodes.Status404NotFound; }

            else if (exceptions is ArgumentException) { return StatusCodes.Status400BadRequest; }

            else if(exceptions is EmailAlreadyInUseException) { return StatusCodes.Status400BadRequest;}

            else { return StatusCodes.Status500InternalServerError; }
        }
        private string GetErrorMessage(Exception exceptions)
        {
            if (exceptions is ArgumentException) return "Invaild Argument";

            else if (exceptions is KeyNotFoundException) return "Value not found";

            else if (exceptions is ArgumentNullException) return "Argument can not be null";

            else if (exceptions is EmailAlreadyInUseException) return "Email already in use";

            else
            {
                return "Unexpected Error";
            }


        }
    }
}
