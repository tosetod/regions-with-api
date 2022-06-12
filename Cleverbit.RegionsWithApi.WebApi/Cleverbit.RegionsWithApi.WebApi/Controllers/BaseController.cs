using Cleverbit.RegionsWithApi.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cleverbit.RegionsWithApi.WebApi.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            if (_mediator == null)
            {
                _mediator = mediator;
            }
        }

        protected async Task<TResponse> Handle<TResponse>(BaseRequest<TResponse> request)
        {
            return await _mediator.Send(request);
        }
    }
}
