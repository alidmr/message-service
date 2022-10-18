using System.Net;
using System.Security.Authentication;
using MessageService.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MessageService.Application.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
            catch (Exception exception)
            {
                await HandleException(context, exception);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            var errorMessage = "İşlem sırasında bir hata oluştu. Daha sonra tekrar deneyiniz";
            HttpStatusCode statusCode;
            var message = exception.Message;

            if (exception is DomainException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = errorMessage;
            }
            else if (exception is ApplicationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else if (exception is AuthenticationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                message = errorMessage;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                message = errorMessage;
            }

            var response = new
            {
                StatusCode = statusCode,
                Message = message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}