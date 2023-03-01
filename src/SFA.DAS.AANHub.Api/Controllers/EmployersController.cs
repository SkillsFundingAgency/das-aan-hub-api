using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember;
using SFA.DAS.AANHub.Application.Employers.Commands.PatchEmployerMember;
using SFA.DAS.AANHub.Application.Employers.Queries;
using SFA.DAS.AANHub.Domain.Entities;

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
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEmployer([FromHeader(Name = Constants.PostRequestHeaders.RequestedByUserHeader)] [Required] Guid requestedByMemberId,
            CreateEmployerModel request)
        {
            _logger.LogInformation("AAN Hub API: Received command to add employer by accountId: {accountId} and UserRef: {UserRef}. Requesting requestedByMemberId: {requestedByMemberId}",
                request.AccountId,
                request.UserRef,
                requestedByMemberId);

            CreateEmployerMemberCommand command = request;
            command.RequestedByMemberId = requestedByMemberId;

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

        /// <summary>
        /// Patch an Employer member
        /// </summary>
        /// <param name="userRef"></param>
        /// <returns></returns>///
        [HttpPatch]
        [Route("{userRef}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchEmployer([FromHeader(Name = Constants.PostRequestHeaders.RequestedByUserHeader), Required] Guid requestedByMemberId, [FromRoute] Guid userRef, [FromBody] JsonPatchDocument<Employer> request)
        {
            _logger.LogInformation("AAN Hub API: Received command to patch Employer by UserName: {userRef} and RequestedByMemberId: {requestedByMemberId}", userRef, requestedByMemberId);

            PatchEmployerMemberCommand command = new()
            {
                RequestedByMemberId = requestedByMemberId,
                UserRef = userRef,
                PatchDoc = request
            };

            var response = await _mediator.Send(command);

            return GetPatchResponse(response);
        }
    }
}