using Cleverbit.RegionsWithApi.Infrastructure.MiddlewareFilters;
using Microsoft.AspNetCore.Builder;

namespace Cleverbit.RegionsWithApi.Infrastructure.ErrorHandling.Extensions
{
    public static class ErrorHandlingFilterExtensions
    {
        public static IApplicationBuilder UseErrorHandlingFilter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalErrorHandlingFilter>();
        }
    }
}
