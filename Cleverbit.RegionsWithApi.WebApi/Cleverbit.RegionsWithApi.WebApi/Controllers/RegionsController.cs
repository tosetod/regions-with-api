using Cleverbit.RegionsWithApi.Core.Features.Regions.Commands;
using Cleverbit.RegionsWithApi.Core.Features.Regions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cleverbit.RegionsWithApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : BaseController
    {
        public RegionsController(IMediator mediator) : base(mediator)
        {
        }

		[AllowAnonymous]
		[HttpPost]
		public async Task<CreateRegion.CommandResult> CreateRegion([FromBody] CreateRegion.Command command)
			=> await Handle(command);

		[AllowAnonymous]
		[HttpGet]
		[Route("{Id}/employees")]
		public async Task<GetAllRegionEmployees.QueryResult> GetAllRegionEmployees([FromRoute] GetAllRegionEmployees.Query query)
			=> await Handle(query);
	}
}
