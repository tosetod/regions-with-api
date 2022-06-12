using MediatR;

namespace Cleverbit.RegionsWithApi.Core
{
    public abstract class BaseRequest<TResponse> : IRequest<TResponse>
    {
    }
}
