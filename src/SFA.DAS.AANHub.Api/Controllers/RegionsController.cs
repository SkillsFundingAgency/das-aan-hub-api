using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.Regions.Queries;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(IMediator mediator, ILogger<RegionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        /// <summary>
        /// Get list of regions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(GetAllRegionsQueryResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRegions()
        {
            _logger.LogTrace("Requesting list of regions");

            var result = await _mediator.Send(new GetAllRegionsQuery());

            _logger.LogTrace("List of regions found");
            return new OkObjectResult(result);

        }
    }
}
