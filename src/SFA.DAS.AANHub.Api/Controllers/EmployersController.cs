using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.Employers.Commands;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployersController : ControllerBase
    {
        private readonly ILogger<EmployersController> _logger;
        private readonly IMediator _mediator;
        public EmployersController(ILogger<EmployersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        /// <summary>
        /// Creates an employer member
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEmployer(CreateEmployersCommand request)
        {
            _logger.LogInformation("AAN Hub API: Received command to add employer by accountId: {accountId} and UserId: {userId}:", request.AccountId, request.UserId);

            var response = await _mediator.Send(request);
            return new CreatedAtActionResult(nameof(CreateEmployer), "Employer", new { id = response.MemberId }, response);
        }
    }
}
