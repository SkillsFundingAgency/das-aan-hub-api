using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Employers.Commands;
using SFA.DAS.AANHub.Application.Employers.Queries;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployersController : ActionResponseControllerBase
    {
        private const string ControllerName = "Employers";
        private readonly ILogger<EmployersController> _logger;
        private readonly IMediator _mediator;

        public EmployersController(ILogger<EmployersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        ///     Creates an employer member
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEmployer([FromHeader(Name = Constants.PostRequestHeaders.RequestedByUserHeader)] [Required] Guid userId,
            CreateEmployerModel request)
        {
            _logger.LogInformation("AAN Hub API: Received command to add employer by accountId: {accountId} and UserId: {userId}. Requesting user: {userId}",
                request.AccountId,
                request.UserRef,
                userId);

            CreateEmployerMemberCommand command = request;
            command.RequestedByMemberId = userId;

            var response = await _mediator.Send(command);

            return GetPostResponse(response,
                new ReferrerRouteDetails(
                    nameof(CreateEmployer),
                    ControllerName,
                    new RouteValueDictionary
                    {
                        {
                            "id", response.Result?.MemberId.ToString()
                        }
                    }));
        }

        /// <summary>
        ///     Gets an employer member
        /// </summary>
        /// <param name="userRef"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/employers/{userRef}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetEmployerMemberResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployer(Guid userRef)
        {
            _logger.LogInformation("AAN Hub API: Received command to get employer by UserRef: {userRef}", userRef);

            var response = await _mediator.Send(new GetEmployerMemberQuery(userRef));

            return GetResponse(response);
        }
    }
}