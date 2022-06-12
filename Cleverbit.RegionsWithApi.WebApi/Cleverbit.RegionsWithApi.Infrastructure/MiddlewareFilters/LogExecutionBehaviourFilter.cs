using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;

namespace Cleverbit.RegionsWithApi.Infrastructure.MiddlewareFilters
{
    public class LogExecutionBehaviorFilter<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public LogExecutionBehaviorFilter(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var correlationId = Guid.NewGuid();
            var timer = new System.Diagnostics.Stopwatch();

            try
            {
                var data = JsonSerializer.Serialize(request);
                using var loggingScope = _logger.BeginScope("{MeditatorRequestName} with {MeditatorRequestData}, correlation id {CorrelationId}", typeof(TRequest).Name, data, correlationId);

                _logger.LogDebug("Handler for {MeditatorRequestName} starting", typeof(TRequest).Name);

                #region Logging request properties (debugging purposes)
                Type requestType = request.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(requestType.GetProperties());

                foreach (PropertyInfo prop in props)
                {
                    object? propValue = prop.GetValue(request, null);
                    _logger.LogDebug("{Property} : {@Value}", prop.Name, propValue);
                }
                #endregion

                timer.Start();
                var result = await next();
                timer.Stop();
                _logger.LogDebug("Handler for {MeditatorRequestName} finished in {ElapsedMilliseconds}ms", typeof(TRequest).Name, timer.Elapsed.TotalMilliseconds);
                _logger.LogInformation("Handled {Name}", typeof(TResponse).Name);

                return result;
            }
            catch (Exception e)
            {
                timer.Stop();
                _logger.LogError(e, "Handler for {MeditatorRequestName} failed in {ElapsedMilliseconds}ms", typeof(TRequest).Name, timer.Elapsed.TotalMilliseconds);
                throw;
            }
        }
    }
}
