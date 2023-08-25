using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Notifications.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers;
public class NotificationsControllerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task GetNotification_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] NotificationsController sut,
        GetNotificationQueryResult notification)
    {
        var response = new ValidatedResponse<GetNotificationQueryResult>(notification);
        mediator.Setup(m => m.Send(It.IsAny<GetNotificationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.Get(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>());
        var resultObjectResult = result as ObjectResult;

        using (new AssertionScope())
        {
            resultObjectResult.Should().NotBeNull();
            resultObjectResult.Should().BeOfType<OkObjectResult>();
            resultObjectResult!.Value.Should().NotBeNull();
            resultObjectResult!.Value.Should().Be(notification);
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task GetNotifications_WithInvalidNotificationId_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] NotificationsController sut)
    {
        var response = new ValidatedResponse<GetNotificationQueryResult>((GetNotificationQueryResult)null!);
        mediator.Setup(m => m.Send(It.IsAny<GetNotificationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.Get(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>());

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test, RecursiveMoqAutoData]
    public async Task GetNotifications_WithInvalidMemberId_ReturnsBadRequestObjectResult(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] NotificationsController sut,
        List<ValidationFailure> errors)
    {
        var errorResponse = new ValidatedResponse<GetNotificationQueryResult>(errors);
        mediator.Setup(m => m.Send(It.IsAny<GetNotificationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);

        var result = await sut.Get(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>());

        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
