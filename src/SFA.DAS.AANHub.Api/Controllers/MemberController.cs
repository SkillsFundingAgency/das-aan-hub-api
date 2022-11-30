using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.Commands.CreateMember;
using SFA.DAS.AANHub.Application.Commands.ModifyMember;
using SFA.DAS.AANHub.Application.Responses;
using SFA.DAS.AANHub.Domain.Enums;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class MemberController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MemberController> _logger;

        public MemberController(IMediator mediator, ILogger<MemberController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("apprentice")]
        public async Task<IActionResult> CreateApprenticeMember([FromBody] CreateMemberCommand request)
        {
            request.UserType = MembershipUserTypes.Apprentice;
            return await CreateMember(request);
        }

        [HttpPost("employer")]
        public async Task<IActionResult> CreateEmployerMember([FromBody] CreateMemberCommand request)
        {
            request.UserType = MembershipUserTypes.Employer;
            return await CreateMember(request);
        }

        [HttpPost("partner")]
        public async Task<IActionResult> CreatePartnerMember([FromBody] CreateMemberCommand request)
        {
            request.UserType = MembershipUserTypes.Partner;
            return await CreateMember(request);
        }

        [HttpPost("admin")]
        public async Task<IActionResult> CreateAdminMember([FromBody] CreateMemberCommand request)
        {
            request.UserType = MembershipUserTypes.Admin;
            return await CreateMember(request);
        }

        [HttpPatch("{memberId}")]
        public async Task<IActionResult> ManageMembership([FromBody] ModifyMemberCommand request, string memberId)
        {
            if (string.IsNullOrEmpty(memberId) || string.IsNullOrEmpty(request.UserId))
            {
                var error = "UserId missing";
                _logger.LogError("{error}", error);
                return BadRequest(error);
            }

            if (memberId != request.UserId)
            {
                var error = "UserId mismatch";
                _logger.LogError("{error}", error);
                return BadRequest(error);
            }

            try
            {
                await _mediator.Send(request);
                return Ok(null);
            }
            catch (Exception e)
            {
                var errorMessage = $"Error attempting to modify member {memberId}";

                if (e.GetType() == typeof(KeyNotFoundException))
                {
                    errorMessage = $"MemberId is unknown {memberId}";
                    _logger.LogError(e, "{errorMessage}", errorMessage);
                    return NotFound(errorMessage);
                }

                _logger.LogError(e, "{errorMessage}", errorMessage);
                return BadRequest(errorMessage);
            }
        }

        private async Task<IActionResult> CreateMember(CreateMemberCommand request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(new CreateMemberApiResponse(result));
            }
            catch (Exception e)
            {
                var error = $"Error attempting to create {request.UserType?.ToString()?.ToLower()} member";
                _logger.LogError(e, "{error}", error);
                return BadRequest();
            }
        }
    }
}
