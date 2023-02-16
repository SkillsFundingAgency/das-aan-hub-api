using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Partners.Commands.CreatePartnerMember;
using SFA.DAS.AANHub.Application.Partners.Commands.PatchPartnerMember;
using SFA.DAS.AANHub.Application.Partners.Queries;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnersController : ActionResponseControllerBase
    {
        private const string ControllerName = "Partners";
        private readonly ILogger<PartnersController> _logger;
        private readonly IMediator _mediator;

        public PartnersController(ILogger<PartnersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        ///     Creates a partner member
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestedByMemberId"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreatePartner([FromHeader(Name = Constants.PostRequestHeaders.RequestedByUserHeader)] [Required] Guid requestedByMemberId,
            CreatePartnerModel request)
        {
            _logger.LogInformation("AAN Hub API: Received command to add partner by member {requestedByMemberId}", requestedByMemberId);

            var command = (CreatePartnerMemberCommand)request;
            command.RequestedByMemberId = requestedByMemberId;

            var response = await _mediator.Send(command);

            return GetPostResponse(response,
                new ReferrerRouteDetails(
                    nameof(CreatePartner),
                    ControllerName,
                    new RouteValueDictionary
                    {
                        {
                            "id", response.Result?.MemberId.ToString()
                        }
                    }));
        }

        /// <summary>
        /// Gets an employer member
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userName}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetPartnerMemberResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPartner(string userName)
        {
            _logger.LogInformation("AAN Hub API: Received command to get partner by userName: {userName}", userName);

            var response = await _mediator.Send(new GetPartnerMemberQuery(userName));

            return GetResponse(response);
        }

        /// <summary>
        /// Patch an partner member
        /// </summary>
        /// <param name="requestedByMemberId"></param>
        /// <param name="userName"></param>/// 
        [HttpPatch]
        [Route("{userName}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchPartner([FromHeader(Name = Constants.PostRequestHeaders.RequestedByUserHeader), Required] Guid requestedByMemberId, [FromRoute] string userName, [FromBody] JsonPatchDocument<Partner> request)
        {
            _logger.LogInformation("AAN Hub API: Received command to patch partner by UserName: {userName} and RequestedByMemberId: {requestedByMemberId}", userName, requestedByMemberId);

            PatchPartnerMemberCommand command = new()
            {
                RequestedByMemberId = requestedByMemberId,
                UserName = userName,
                PatchDoc = request
            };

            var response = await _mediator.Send(command);

            return GetPatchResponse(response);
        }
    }
}