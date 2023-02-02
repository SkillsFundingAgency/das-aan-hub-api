using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Partners;

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
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreatePartner([FromHeader(Name = Constants.PostRequestHeaders.RequestedByUserHeader)] [Required] Guid? userId,
            CreatePartnerModel request)
        {
            _logger.LogInformation("AAN Hub API: Received command to add partner by member {userId}", userId);

            var command = (CreatePartnerMemberCommand)request;
            command.RequestedByMemberId = userId;

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
    }
}