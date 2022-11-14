
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AAN.Application.Commands.CreateMember;
using SFA.DAS.AAN.Hub.Api.Requests;
using SFA.DAS.AAN.Hub.Api.Responses;


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
        public async Task<IActionResult> CreateApprenticeMember([FromBody] CreateMemberRequest request)
        {
            try
            {
                CreateMemberResponse result = await _mediator.Send(
                    new CreateMemberCommand(request.id, "Apprentice", request.joined, request.region, request.information, request.organisation)
                );
                return Ok(new CreateMemberApiResponse(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to create apprentice member");
                return BadRequest();
            }
        }

        [HttpPost("employer")]
        public async Task<IActionResult> CreateApprenticeEmployer([FromBody] CreateMemberRequest request)
        {
            try
            {
                CreateMemberResponse result = await _mediator.Send(
                    new CreateMemberCommand(request.id, "Employer", request.joined, request.region, request.information, request.organisation)
                );
                return Ok(new CreateMemberApiResponse(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to create apprentice member");
                return BadRequest();
            }
        }

        [HttpPost("partner")]
        public async Task<IActionResult> CreateApprenticePartner([FromBody] CreateMemberRequest request)
        {
            try
            {
                CreateMemberResponse result = await _mediator.Send(
                    new CreateMemberCommand(request.id, "Partner", request.joined, request.region, request.information, request.organisation)
                );
                return Ok(new CreateMemberApiResponse(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to create partner member");
                return BadRequest();
            }
        }
    }
}
