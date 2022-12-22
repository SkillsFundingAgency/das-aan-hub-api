using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
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
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEmployer([FromHeader(Name = Constants.PostRequestHeaders.RequestedByUserHeader), Required] Guid? userId, CreateEmployerModel request)
        {
            _logger.LogInformation("AAN Hub API: Received command to add employer by accountId: {accountId} and UserId: {userId}:", request.AccountId, request.UserId);

            CreateEmployerMemberCommand command = request;
            command.RequestedByUserId = userId;

            var response = await _mediator.Send(command);
            return new CreatedAtActionResult(nameof(CreateEmployer), "Employer", new { id = response.MemberId }, response);
        }
    }
}
