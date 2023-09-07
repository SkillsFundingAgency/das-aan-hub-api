using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Notifications.Commands;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.NotificationsControllerTests;
public class NotificationsControllerCreateTests
{
    [Test, MoqAutoData]
    public async Task CreateNotification_InvokesRequest_Returns201Response(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] NotificationsController sut,
        CreateNotificationCommand command,
        CreateNotificationModel model,
        CreateNotificationCommandResponse response)
    {
        command.MemberId = model.MemberId;
        command.NotificationTemplateId = command.NotificationTemplateId;
        mediator.Setup(m => m.Send(It.IsAny<CreateNotificationCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidatedResponse<CreateNotificationCommandResponse>(response));

        var result = await sut.CreateNotification(command.RequestedByMemberId, model, It.IsAny<CancellationToken>()) as CreatedAtActionResult;

        using (new AssertionScope())
        {
            result!.ControllerName.Should().Be("Notifications");
            result.ActionName.Should().Be("Get");
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Value.Should().BeOfType<CreateNotificationCommandResponse>();
            result.Value.As<CreateNotificationCommandResponse>().NotificationId.Should().NotBeEmpty();
        }
    }

    [Test, MoqAutoData]
    public async Task CreateNotification_InvokesRequestWithValidationErrors_ReturnsBadRequest(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] NotificationsController sut,
        CreateNotificationCommand command,
        CreateNotificationModel model)
    {
        var errorResponse = new ValidatedResponse<CreateNotificationCommandResponse>
            (new List<ValidationFailure>
            {
                new ValidationFailure("Name", "error")
            });
        mediator.Setup(m => m.Send(It.IsAny<CreateNotificationCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);

        var result = await sut.CreateNotification(command.RequestedByMemberId, model, It.IsAny<CancellationToken>());
        var resultObject = result as BadRequestObjectResult;
        var errorList = resultObject?.Value as List<ValidationFailure>;

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            errorList?.Count.Should().Be(errorResponse.Errors.Count);
            resultObject?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}
