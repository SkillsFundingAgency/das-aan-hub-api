using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;
using SFA.DAS.AANHub.Application.Admins.Commands.PatchAdminMember;
using SFA.DAS.AANHub.Application.Admins.Queries;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ActionResponseControllerBase
    {
        private const string ControllerName = "Admins";
        private readonly ILogger<AdminsController> _logger;
        private readonly IMediator _mediator;

        public AdminsController(ILogger<AdminsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        ///     Creates an admin member
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAdmin([FromHeader(Name = Constants.PostRequestHeaders.RequestedByUserHeader)] [Required] Guid userId,
            CreateAdminModel request)
        {
            _logger.LogInformation("AAN Hub API: Received command to add admin by UserId: {userId}", userId);

            var command = (CreateAdminMemberCommand)request;
            command.RequestedByMemberId = userId;

            var response = await _mediator.Send(command);

            return GetPostResponse(response,
                new ReferrerRouteDetails(
                    nameof(CreateAdmin),
                    ControllerName,
                    new RouteValueDictionary
                    {
                        {
                            "id", response.Result?.MemberId.ToString()
                        }
                    }));
        }

        /// <summary>
        ///     Gets an Admin member
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userName}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetAdminMemberResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAdminMemberResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GetAdminMemberResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAdmin(string userName)
        {
            _logger.LogInformation("AAN Hub API: Received command to get Admin by Username: {userName}", userName);

            var response = await _mediator.Send(new GetAdminMemberQuery(userName));

            return GetResponse(response);
        }

        /// <summary>
        ///     Patch an admin member
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="request"></param>
        /// ///
        [HttpPatch]
        [Route("{userName}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchAdmin([FromHeader(Name = Constants.PostRequestHeaders.RequestedByUserHeader)] [Required] Guid userId,
            [FromRoute] string userName, [FromBody] JsonPatchDocument<Admin> request)
        {
            _logger.LogInformation("AAN Hub API: Received command to patch admin by user name: {userName}. RequestedByUser: {userId}",
                userName,
                userId);

            PatchAdminMemberCommand command = new()
            {
                RequestedByMemberId = userId,
                UserName = userName,
                PatchDoc = request
            };

            var response = await _mediator.Send(command);

            return GetPatchResponse(response);
        }
    }
}