using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Partners.Commands.CreatePartnerMember;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.PartnersControllerTests;

public class PartnersControllerCreateTests
{
    private readonly PartnersController _controller;
    private readonly Mock<IMediator> _mediator;

    public PartnersControllerCreateTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new PartnersController(Mock.Of<ILogger<PartnersController>>(), _mediator.Object);
    }

    [Test]
    [MoqAutoData]
    public async Task CreatePartners_InvokesRequest(
        CreatePartnerMemberCommand command)
    {
        var response = new ValidatedResponse<CreateMemberCommandResponse>
        (new CreateMemberCommandResponse(command.Id));

        _mediator.Setup(m => m.Send(It.IsAny<CreatePartnerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var result = await _controller.CreatePartner(command) as CreatedAtActionResult;

        result?.ControllerName.Should().Be("Partners");
        result?.ActionName.Should().Be("Get");
        result?.StatusCode.Should().Be(StatusCodes.Status201Created);
        result?.Value.As<CreateMemberCommandResponse>().MemberId.Should().Be(command.Id);
    }

    [Test]
    [MoqAutoData]
    public async Task CreatePartner_InvokesRequest_WithErrors()
    {
        var errorResponse = new ValidatedResponse<CreateMemberCommandResponse>
        (new List<ValidationFailure>
        {
            new("Name", "error")
        });


        var command = new CreatePartnerMemberCommand();
        _mediator.Setup(m => m.Send(It.IsAny<CreatePartnerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);
        var result = await _controller.CreatePartner(command);

        result.As<BadRequestObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
}
