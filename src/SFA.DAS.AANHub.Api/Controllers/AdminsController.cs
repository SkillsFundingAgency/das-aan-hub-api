using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AdminsController : ActionResponseControllerBase
{
    private readonly IMediator _mediator;

    public override string ControllerName => "Members";

    public AdminsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAdmin(CreateAdminMemberRequestModel model)
    {
        CreateAdminMemberCommand command = model;

        var response = await _mediator.Send(command);

        return GetPostResponse(response, new { email = command.Email });
    }
}