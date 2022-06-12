using Cleverbit.RegionsWithApi.Data.Entities;
using FluentValidation;

namespace Cleverbit.RegionsWithApi.Core.Features.Regions.Commands
{
    public class CreateRegion
    {
        public class Command : BaseRequest<CommandResult>
        {
            public string Name { get; set; }
            public int? ParentRegionId { get; set; }
        }

        public class CommandResult
        {
        }

        public class Validator : BaseValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
            }
        }

        public class CommandHandler : BaseHandler<Command, CommandResult>
        {

            public override async Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
            {
                var region = new Region
                {
                    Name = command.Name,
                    ParentRegionId = command.ParentRegionId
                };

                Add(region);

                return new CommandResult();
            }
        }
    }
}
