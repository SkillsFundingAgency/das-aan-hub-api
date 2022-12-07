using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.Apprentices.Commands;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprenticesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateApprentice(CreateApprenticesCommand request)
        {
            var response = await _mediator.Send(request);
            return new CreatedAtActionResult(nameof(MemberController.GetMember), "Member", new { id = response }, new { memberId = response, status = "Live" } );
        }
    }
}
