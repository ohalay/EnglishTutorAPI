namespace EnglishTutor.Common.Exception
{
    public class ApiError
    {
        public static ApiError InternalServerError = new ApiError{HttpCode = 500, Reason = "internal_server_error" };
        public static ApiError AccessForbidden = new ApiError { HttpCode = 403, Reason = "access_forbidden" };
        public static ApiError Unauthorized = new ApiError { HttpCode = 401, Reason = "unauthorized" };

        public string Reason { get; set; }
        public int HttpCode { get; set; }
    }
}