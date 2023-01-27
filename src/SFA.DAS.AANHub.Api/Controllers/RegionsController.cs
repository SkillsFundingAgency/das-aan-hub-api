using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Regions.Queries.GetRegions;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : ActionResponseControllerBase
    {
        private readonly ILogger<RegionsController> _logger;
        private readonly IMediator _mediator;

        public RegionsController(IMediator mediator, ILogger<RegionsController> logger) : base(logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        ///     Get list of regions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(GetRegionsQueryResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRegions()
        {
            _logger.LogTrace("Requesting list of regions");

            var result = await _mediator.Send(new GetRegionsQuery());

            return GetResponse(result,
                new BaseRequestDetails
                {
                    ActionName = nameof(GetRegions),
                    ControllerName = nameof(RegionsController)
                });
        }
    }
}