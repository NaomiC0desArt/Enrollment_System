using System.Diagnostics;
using System.Net;
using UniversitySystem.Application.DTOs.ApiResponse;
using UniversitySystem.Application.Exceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UniversitySystem.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware (RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "An unexpected error ocurred";
            List<string>? errorList = null;

            if (exception is UserFriendlyException ufe)
            {
                statusCode = (int)ufe.StatusCode;
                message = ufe.Message;
            }
            else if(exception is FluentValidation.ValidationException ve)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = "Validation failed";
                errorList = ve.Errors.Select(e => e.ErrorMessage).ToList();
            }
            else if (_env.IsDevelopment())
            {
                message = exception.Message;
                errorList = new List<string> { exception.StackTrace ?? "" };
            }

            context.Response.StatusCode = statusCode;

            var response = ApiResponse<object>.Fail(message, errorList);

            await context.Response.WriteAsJsonAsync(response);
        }


    }
}
