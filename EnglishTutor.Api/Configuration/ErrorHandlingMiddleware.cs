using System;
using System.Threading.Tasks;
using EnglishTutor.Common;
using EnglishTutor.Common.Exception;
using log4net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EnglishTutor.Api.Configuration
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILog logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var apiException = exception as ApiException ??
                               new ApiException(ApiError.InternalServerError) { ErrorData = exception.Message};

            _logger.Error(apiException.ErrorData);

            var result = JsonConvert.SerializeObject(apiException.ToReportError());
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = apiException.Error.HttpCode;
            return context.Response.WriteAsync(result);
        }
    }
}
