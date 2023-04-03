using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.StagedApprentices.Queries;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StagedApprenticesController : ActionResponseControllerBase
    {
        private readonly ILogger<StagedApprenticesController> _logger;
        private readonly IMediator _mediator;

        public StagedApprenticesController(ILogger<StagedApprenticesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        ///     Gets an StagedApprentice member
        /// </summary>
        /// <param name="lastname"></param>
        /// <param name="dateofbirth"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/StagedApprentice/lastname={lastname}&dateofbirth={dateofbirth}&email={email}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GetStagedApprenticeResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStagedApprentice(string lastname, DateTime dateofbirth, string email)
        {
            _logger.LogInformation("AAN Hub API: Received command to get StagedApprentice by LastName: {lastname}, DateOfBirth: {dateofbirth} and Email: {email}", lastname, dateofbirth, email);

            var response = await _mediator.Send(new GetStagedApprenticeQuery(lastname, dateofbirth, email));
            return GetResponse(response);
        }
    }
}