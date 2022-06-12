using Cleverbit.RegionsWithApi.Infrastructure.MiddlewareFilters;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cleverbit.RegionsWithApi.Infrastructure.Logging.Extensions
{
    public static class LogExecutionBehaviorExtensions
    {
        public static IServiceCollection AddRequestsLoggingFilter(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LogExecutionBehaviorFilter<,>));

            return services;
        }
    }
}
