namespace WMS.Presentation
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public object? Error { get; set; }

        public static ApiResponse<T> SuccessResponse(T? data = default, string message = "Success", string code = "SUCCESS")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Code = code,
                Message = message,
                Data = data,
                Error = null
            };
        }

        public static ApiResponse<T> FailureResponse(string message, string code = "ERROR", object? error = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Code = code,
                Message = message,
                Data = default,
                Error = error
            };
        }
    }
}
