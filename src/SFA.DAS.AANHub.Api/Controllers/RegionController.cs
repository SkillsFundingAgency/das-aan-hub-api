using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.Queries.GetAllRegions;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class RegionController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RegionController> _logger;

        public RegionController(IMediator mediator, ILogger<RegionController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetAllRegionsResult), 200)]
        public async Task<IActionResult> GetListOfRegions()
        {
            var result = await _mediator.Send(new GetAllRegionsQuery());
            _logger.LogInformation("List of regions found");
            return new OkObjectResult(result);

        }
    }
}
