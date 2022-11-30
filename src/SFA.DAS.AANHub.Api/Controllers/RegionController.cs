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
        public async Task<IActionResult> GetListOfRegions()
        {
            try
            {
                var result = await _mediator.Send(new GetAllRegionsQuery());
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve list of regions");
                return BadRequest();
            }
        }
    }
}
