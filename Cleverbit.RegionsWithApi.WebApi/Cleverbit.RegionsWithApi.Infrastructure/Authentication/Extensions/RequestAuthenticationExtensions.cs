using Cleverbit.RegionsWithApi.Infrastructure.MiddlewareFilters;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cleverbit.RegionsWithApi.Infrastructure.Authentication.Extensions
{
    public static class RequestAuthenticationExtensions
    {
        public static IServiceCollection AddRequestsAuthenticationFilter(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestAuthenticationFilter<,>));

            return services;
        }
    }
}
