using Cleverbit.RegionsWithApi.Common.Exceptions;
using Cleverbit.RegionsWithApi.Core;
using Cleverbit.RegionsWithApi.Infrastructure.Authentication.Generator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Cleverbit.RegionsWithApi.Infrastructure.MiddlewareFilters
{
    public class RequestAuthenticationFilter<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly bool? _authenticated = false;
        private readonly bool _allowAnonymous = false;

        public RequestAuthenticationFilter(IHttpContextAccessor httpContextAccessor)
        {
            var endpoint = httpContextAccessor.HttpContext?.GetEndpoint();
            _allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object;

            _authenticated = httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response = null;

            if (_authenticated == true || _allowAnonymous)
            {
                response = await next();

                try
                {
                    if (response.GetType().BaseType?.Name == nameof(AuthenticationResponse))
                    {
                        var authResponse = response as AuthenticationResponse;
                        AuthenticationTokenGenerator.GenerateAuthenticationToken(authResponse);
                        authResponse?.Validate();
                        response = authResponse as TResponse;
                    }
                }
                catch (Exception ex)
                {
                    throw new CoreException(ex.Message);
                }
            }

            return response;
        }
    }
}
