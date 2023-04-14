using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Application.Apprentices.Commands.PatchApprenticeMember;
using SFA.DAS.AANHub.Application.Apprentices.Queries;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprenticesController : ActionResponseControllerBase
    {
        private const string ControllerName = "Apprentices";
        private readonly ILogger<ApprenticesController> _logger;
        private readonly IMediator _mediator;

        public ApprenticesController(ILogger<ApprenticesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        ///     Creates an apprentice member
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateApprentice(CreateApprenticeModel request)
        {
            CreateApprenticeMemberCommand command = request;

            var response = await _mediator.Send(command);

            return GetPostResponse(response,
                new ReferrerRouteDetails(
                    nameof(GetApprentice),
                    ControllerName,
                    new RouteValueDictionary
                    {
                        {
                            "apprenticeId", request.ApprenticeId
                        }
                    }));
        }

        /// <summary>
        ///     Gets an apprentice member
        /// </summary>
        /// <param name="apprenticeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{apprenticeId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GetApprenticeMemberResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetApprentice(Guid apprenticeId)
        {
            _logger.LogInformation("AAN Hub API: Received command to get apprentice by ApprenticeId: {apprenticeId}", apprenticeId);

            var response = await _mediator.Send(new GetApprenticeMemberQuery(apprenticeId));
            return GetResponse(response);
        }

        /// <summary>
        ///     Patch an apprentice member
        /// </summary>
        /// <param name="requestedByMemberId"></param>
        /// <param name="apprenticeId"></param>
        /// <param name="request"></param>
        /// ///
        [HttpPatch]
        [Route("{apprenticeId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchApprentice([FromHeader(Name = Constants.PostRequestHeaders.RequestedByUserHeader)][Required] Guid requestedByMemberId,
            [FromRoute] Guid apprenticeId, [FromBody] JsonPatchDocument<Apprentice> request)
        {
            _logger.LogInformation("AAN Hub API: Received command to patch apprentice by ApprenticeId: {apprenticeId} and UserId: {userId}",
                apprenticeId,
                requestedByMemberId);

            PatchApprenticeMemberCommand command = new()
            {
                RequestedByMemberId = requestedByMemberId,
                ApprenticeId = apprenticeId,
                PatchDoc = request
            };

            var response = await _mediator.Send(command);

            return GetPatchResponse(response);
        }
    }
}