using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Admins.Commands;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly ILogger<AdminsController> _logger;
        private readonly IMediator _mediator;
        public AdminsController(ILogger<AdminsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        /// <summary>
        /// Creates an admin member
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAdmin([FromHeader(Name = Constants.PostRequestHeaders.RequestedByUserHeader), Required] Guid? userId, CreateAdminModel request)
        {
            _logger.LogInformation("AAN Hub API: Received command to add admin by UserId: {userId}:", userId);

            CreateAdminMemberCommand command = request;
            command.RequestedByMemberId = userId;

            var response = await _mediator.Send(command);
            return response.IsValidResponse ? new CreatedAtActionResult(nameof(CreateAdmin), "Admins", new { id = response.Result.MemberId }, response.Result)
                : new BadRequestObjectResult(response.Errors);
        }
    }
}
