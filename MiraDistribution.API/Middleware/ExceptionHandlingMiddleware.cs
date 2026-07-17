using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Domain.Exceptions;
using System.Net;
using System.Text.Json;
using ValidationException = MiraDistribution.Application.Common.Exceptions.ValidationException;

namespace MiraDistribution.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            object response;

            switch (exception)
            {
                case ValidationException validationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new { title = "خطأ في البيانات المدخلة", errors = validationException.Errors };
                    break;

                case NotFoundException notFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response = new { title = notFoundException.Message };
                    break;

                case AuthenticationException authException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response = new { title = authException.Message };
                    break;

                case ForbiddenAccessException forbiddenException:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    response = new { title = forbiddenException.Message };
                    break;

                case DomainException domainException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new { title = domainException.Message };
                    break;
                
                default:
                    _logger.LogError(exception, "حصل خطأ غير متوقع");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = new { title = "حصل خطأ غير متوقع، حاول تاني." };
                    break;
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}