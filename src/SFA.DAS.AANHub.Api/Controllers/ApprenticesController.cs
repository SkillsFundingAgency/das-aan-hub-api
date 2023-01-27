using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Apprentices.Commands;
using SFA.DAS.AANHub.Application.Apprentices.Queries;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprenticesController : ActionResponseControllerBase
    {
        private readonly ILogger<ApprenticesController> _logger;
        private readonly IMediator _mediator;

        public ApprenticesController(ILogger<ApprenticesController> logger, IMediator mediator) : base(logger)
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
        public async Task<IActionResult> CreateApprentice([FromHeader(Name = Constants.PostRequestHeaders.RequestedByUserHeader)] [Required] Guid? userId,
            CreateApprenticeModel request)
        {
            _logger.LogInformation("AAN Hub API: Received command to add apprentice by ApprenticeId: {apprenticeId} and UserId: {userId}",
                request.ApprenticeId,
                userId);

            CreateApprenticeMemberCommand command = request;
            command.RequestedByMemberId = userId;

            var response = await _mediator.Send(command);

            return GetPostResponse(response,
                new BaseRequestDetails
                {
                    ActionName = nameof(GetApprentice),
                    ControllerName = "Apprentices",
                    GetParameters = response.Errors.Any()
                        ? null
                        : new RouteValueDictionary
                        {
                            {
                                "apprenticeId", request.ApprenticeId
                            }
                        }
                });
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
        public async Task<IActionResult> GetApprentice(long apprenticeId)
        {
            _logger.LogInformation("AAN Hub API: Received command to get apprentice by ApprenticeId: {apprenticeId}", apprenticeId);

            var response = await _mediator.Send(new GetApprenticeMemberQuery(apprenticeId));
            return GetResponse(response,
                new BaseRequestDetails
                {
                    ActionName = nameof(GetApprentice),
                    ControllerName = nameof(ApprenticesController),
                    GetId = apprenticeId.ToString()
                });
        }
    }
}