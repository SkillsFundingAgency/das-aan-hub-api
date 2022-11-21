
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AAN.Application.ApiResponses;
using SFA.DAS.AAN.Application.Commands.CreateMember;
using SFA.DAS.AAN.Domain.Enums;


namespace SFA.DAS.AAN.Hub.Api.Controllers
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

        private async Task<IActionResult> CreateMember(CreateMemberCommand request)
        {
            try
            {
                CreateMemberResponse result = await _mediator.Send(request);
                return Ok(new CreateMemberApiResponse(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to create {request.UserType?.ToString()?.ToLower()} member");
                return BadRequest();
            }
        }
    }
}
