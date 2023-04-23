using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Employers.Queries;
using SFA.DAS.AANHub.Application.Mediatr.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.EmployersControllerTests;

public class EmployersControllerGetTests
{
    [Test]
    [MoqAutoData]
    public async Task GetEmployer_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployersController sut,
        Guid userRef)
    {
        await sut.GetEmployer(userRef);

        mediatorMock.Verify(m => m.Send(It.Is<GetEmployerMemberQuery>(q => q.UserRef == userRef), It.IsAny<CancellationToken>()));
    }

    [Test]
    [MoqAutoData]
    public async Task GetEmployer_HandlerReturnsNullResult_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployersController sut,
        Guid userRef)
    {
        var notFoundResponse = ValidatedResponse<GetMemberResult>.EmptySuccessResponse();
        mediatorMock.Setup(m => m.Send(It.Is<GetEmployerMemberQuery>(q => q.UserRef == userRef), It.IsAny<CancellationToken>())).ReturnsAsync(notFoundResponse);

        var result = await sut.GetEmployer(userRef);

        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task GetEmployer_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployersController sut,
        Guid userRef,
        GetMemberResult getMemberResult)
    {
        var response = new ValidatedResponse<GetMemberResult>(getMemberResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetEmployerMemberQuery>(q => q.UserRef == userRef), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.GetEmployer(userRef);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(getMemberResult);
    }

    [Test]
    [MoqAutoData]
    public async Task GetEmployer_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployersController sut,
        List<ValidationFailure> errors,
        Guid userRef)
    {
        var response = new ValidatedResponse<GetMemberResult>(errors);
        mediatorMock.Setup(m => m.Send(It.Is<GetEmployerMemberQuery>(q => q.UserRef == userRef), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.GetEmployer(userRef);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}
