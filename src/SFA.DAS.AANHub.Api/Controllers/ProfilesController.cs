using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ActionResponseControllerBase
    {
        private const string ControllerName = "Profiles";
        private readonly ILogger<AdminsController> _logger;
        private readonly IMediator _mediator;
        public ProfilesController(ILogger<AdminsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Get list of profiles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetProfilesQueryResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfiles()
        {
            _logger.LogInformation("AAN Hub API: Received command to get profiles");

            var result = await _mediator.Send(new GetProfilesQuery());

            return GetResponse(result);
        }

    }
}