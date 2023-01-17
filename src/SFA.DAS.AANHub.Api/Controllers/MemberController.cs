using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.Commands.CreateMember;
using SFA.DAS.AANHub.Application.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]/")]
    public class MemberController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MemberController> _logger;

        public MemberController(IMediator mediator, ILogger<MemberController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Member> GetMember(Guid id) => Ok(new Member() { Id = id });

        [HttpPost("partner")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CreateMemberApiResponse), 200)]
        public async Task<IActionResult> CreatePartnerMember([FromBody] CreateMemberCommand request)
        {
            request.UserType = MembershipUserType.Partner;
            return await CreateMember(request);
        }

        [HttpPost("admin")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CreateMemberApiResponse), 200)]
        public async Task<IActionResult> CreateAdminMember([FromBody] CreateMemberCommand request)
        {
            request.UserType = MembershipUserType.Admin;
            return await CreateMember(request);
        }

        private async Task<IActionResult> CreateMember(CreateMemberCommand request)
        {
            var result = await _mediator.Send(request);
            _logger.LogInformation("Member created: {memberId}", result.Member?.Id);
            return new OkObjectResult(new CreateMemberApiResponse(result));
        }
    }
}
