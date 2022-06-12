using Cleverbit.RegionsWithApi.Common.Exceptions;
using Cleverbit.RegionsWithApi.Common.Utility.Consts;
using Cleverbit.RegionsWithApi.Core;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cleverbit.RegionsWithApi.Infrastructure.Mediator
{
    public class MediatorPipelineHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IRequestHandler<TRequest, TResponse> _handler;

        public MediatorPipelineHandler(
            IRequestHandler<TRequest, TResponse> handler,
            IHttpContextAccessor httpContextAccessor)
        {
            _handler = handler;

            SetupCurrentUserModel(httpContextAccessor);
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            TResponse response;
            try
            {
                response = await _handler.Handle(request, CancellationToken.None);

                try
                {
                    _handler.GetType()
                            .GetMethod(nameof(BaseHandler<BaseRequest<TResponse>, TResponse>.SaveChanges))
                            .Invoke(_handler, null);
                }
                catch (Exception)
                {
                    throw new CoreException("Cannot store your data in our database. Please try again later");
                }
            }
            catch (ResourceNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("CoreException", ex.Message)
                });
            }

            return response;
        }

        private void SetupCurrentUserModel(IHttpContextAccessor httpContextAccessor)
        {
            if (_handler.GetType().BaseType?.Name == typeof(BaseHandler<,>).Name
                || _handler.GetType().BaseType?.BaseType?.Name == typeof(BaseHandler<,>).Name)
            {
                if (httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
                {
                    var currentUser = httpContextAccessor.HttpContext.User;

                    if (!string.IsNullOrEmpty(currentUser.FindFirst(AppClaimTypes.UserId)?.Value))
                    {
                        var userEmail = currentUser?.FindFirst(x => x.Type == AppClaimTypes.Email)?.Value;
                        var userId = int.Parse(currentUser.FindFirst(AppClaimTypes.UserId)?.Value);

                        var currentUserModel = new CurrentUserModel(userId, userEmail);

                        _handler.GetType()
                                .GetProperty(nameof(BaseHandler<BaseRequest<TResponse>, TResponse>._currentLoggedInUser))?
                                .SetValue(_handler, currentUserModel);
                    }
                }
            }
        }
    }
}
