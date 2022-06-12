using Cleverbit.RegionsWithApi.Core.Features.Employees.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cleverbit.RegionsWithApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeesController : BaseController
    {
        public EmployeesController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<CreateEmployee.CommandResult> CreateEmployee([FromBody] CreateEmployee.Command command)
            => await Handle(command);
    }
}
