namespace UploadApi.Exceptions
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public string ErrorType { get; set;}

        public string? Detail { get; set; }

        public ErrorResponse(int statusCode, string message, string errorType, string? detail)
        {
            StatusCode = statusCode;
            Message = message;
            ErrorType = errorType;
            Detail = detail;
        }
    }
}
