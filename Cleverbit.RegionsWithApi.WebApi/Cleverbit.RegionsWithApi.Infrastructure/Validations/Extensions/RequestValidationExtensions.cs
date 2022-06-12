using Cleverbit.RegionsWithApi.Infrastructure.MiddlewareFilters;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cleverbit.RegionsWithApi.Infrastructure.Validations.Extensions
{
    public static class RequestValidationExtensions
    {
        public static IServiceCollection AddRequestsValidationFilter(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationFilter<,>));

            return services;
        }
    }
}
