using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.EmployersControllerTests;

public class EmployersControllerCreateTests
{
    private readonly EmployersController _controller;
    private readonly Mock<IMediator> _mediator;

    public EmployersControllerCreateTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new EmployersController(Mock.Of<ILogger<EmployersController>>(), _mediator.Object);
    }

    [Test]
    [MoqAutoData]
    public async Task CreateEmployer_InvokesRequest(
        CreateEmployerModel model,
        CreateEmployerMemberCommand command)
    {
        var response = new ValidatedResponse<CreateMemberCommandResponse>(new CreateMemberCommandResponse(command.Id));

        _mediator.Setup(m => m.Send(It.IsAny<CreateEmployerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var result = await _controller.CreateEmployer(model) as CreatedAtActionResult;

        result?.ControllerName.Should().Be("Employers");
        result?.ActionName.Should().Be("CreateEmployer");
        result?.StatusCode.Should().Be(StatusCodes.Status201Created);
        result?.Value.As<CreateMemberCommandResponse>().MemberId.Should().Be(command.Id);
    }

    [Test]
    [MoqAutoData]
    public async Task CreateEmployer_InvokesRequest_WithErrors()
    {
        var errorResponse = new ValidatedResponse<CreateMemberCommandResponse>
        (new List<ValidationFailure>
        {
            new("Name", "error")
        });

        var model = new CreateEmployerModel();
        _mediator.Setup(m => m.Send(It.IsAny<CreateEmployerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);
        var result = await _controller.CreateEmployer(model);

        result.As<BadRequestObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
}
