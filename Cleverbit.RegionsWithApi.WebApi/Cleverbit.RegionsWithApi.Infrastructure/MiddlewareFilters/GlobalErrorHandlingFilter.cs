using Cleverbit.RegionsWithApi.Common.Exceptions;
using Cleverbit.RegionsWithApi.Common.Models;
using Cleverbit.RegionsWithApi.Common.Models.Enums;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Cleverbit.RegionsWithApi.Infrastructure.MiddlewareFilters
{
    public class GlobalErrorHandlingFilter
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingFilter(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == 404)
                    throw new CoreException($"The call you made was not found. For all available endpoints, check <a href=\"/swagger\">swagger</a>");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var errorCode = ErrorCode.None;

            string message;
            if (exception is CoreException ce)
            {
                code = HttpStatusCode.BadRequest;
                message = ce.Message;
            }
            else if (exception is FluentValidation.ValidationException ve)
            {
                code = HttpStatusCode.BadRequest;

                var error = ve.Errors.FirstOrDefault();
                message = error != null ? error.ErrorMessage : ve.Message;
            }
            else if (exception is ResourceNotFoundException re)
            {
                code = HttpStatusCode.NotFound;
                message = re.Message;
            }
            else
            {
                message = exception.Message;
            }

            var result = JsonSerializer.Serialize(new ErrorResponse { Message = message, ErrorCode = errorCode });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
