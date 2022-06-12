using Cleverbit.RegionsWithApi.Data.Entities;
using FluentValidation;

namespace Cleverbit.RegionsWithApi.Core.Features.Employees.Commands
{
    public class CreateEmployee
    {
        public class Command : BaseRequest<CommandResult>
        {
            public int RegionId { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
        }

        public class CommandResult
        {
        }

        public class Validator : BaseValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.RegionId)
                    .NotEmpty();
                RuleFor(x => x.Name)
                    .NotEmpty();
                RuleFor(x => x.Surname)
                    .NotEmpty();
            }
        }

        public class CommandHandler : BaseHandler<Command, CommandResult>
        {

            public override async Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
            {
                var employee = new Employee
                {
                    RegionId = command.RegionId,
                    Name = command.Name,
                    Surname = command.Surname
                };

                var employeeId = Add(employee);

                return new CommandResult();
            }
        }
    }
}
