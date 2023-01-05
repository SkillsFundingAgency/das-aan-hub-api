using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SFA.DAS.AANHub.Application.Apprentices.Commands;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprenticesController : ControllerBase
    {
        private readonly ILogger<ApprenticesController> _logger;
        private readonly IMediator _mediator;
        public ApprenticesController(ILogger<ApprenticesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Creates an apprentice member
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateApprentice([FromHeader(Name = Constants.PostRequestHeaders.RequestedByUserHeader), Required] Guid? userId, CreateApprenticeModel request)
        {
            _logger.LogInformation($"AAN Hub API: Received command to add apprentice by ApprenticeId: {request.ApprenticeId} and UserId: {userId}");

            CreateApprenticeMemberCommand command = request;
            command.RequestedByMemberId = userId;

            var response = await _mediator.Send(command);
            //return new CreatedAtActionResult(nameof(MemberController.GetMember), "Member", new { id = response.MemberId }, response);
            return new CreatedAtActionResult(nameof(CreateApprentice), "Apprentice", new { id = response.MemberId }, response);
        }
    }
}

