namespace NotificationsApi.DTO
{
    public class ApiResponse<T>
    {

        public bool Succeed { get; set; }
        public string? ErrorMessage { get; set; }

        public string? ErrorCode { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }

        public static ApiResponse<T> Success(T data, string? message = null)
        {
            return new ApiResponse<T>
            {
                Succeed = true,
                Data = data,
                Message = message
            };
        }

        public static ApiResponse<T> Fail(string errorMessage, string? errorCode = null)
        {
            return new ApiResponse<T>
            {
                Succeed = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
