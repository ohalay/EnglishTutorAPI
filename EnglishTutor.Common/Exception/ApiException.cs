
namespace EnglishTutor.Common.Exception
{
    public class ApiException : System.Exception
    {
        public ApiException(ApiError error): base(error.Reason)
        {
            Error = error;
        }

        public ApiError Error { get; set; }

        public object ErrorData { get; set; }

        public object ToReportError()
        {
            return new
            {
                Error,
                ErrorData
            };
        }
    }
}
