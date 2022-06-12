using Cleverbit.RegionsWithApi.Core;
using FluentValidation;
using Lamar;
using MediatR;

namespace Cleverbit.RegionsWithApi.Infrastructure.Mediator
{
    public class MediatorPipelineRegistry : ServiceRegistry
    {
        public MediatorPipelineRegistry()
        {
            Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.AssemblyContainingType(typeof(BaseHandler<,>));
                scanner.AssemblyContainingType(typeof(IRequestHandler<,>));

                scanner.ConnectImplementationsToTypesClosing(typeof(IValidator<>));
                scanner.ConnectImplementationsToTypesClosing(typeof(BaseValidator<>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
            });

            For<IMediator>().Use<MediatR.Mediator>();
            For(typeof(IRequestHandler<,>)).DecorateAllWith(typeof(MediatorPipelineHandler<,>));
        }
    }
}
