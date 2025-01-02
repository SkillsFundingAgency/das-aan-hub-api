using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.MemberNotificationLocations.Commands.UpdateMemberNotificationSettings;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class MemberNotificationSettingsControllerTests
    {
        [Test, MoqAutoData]
        public async Task PostMemberNotificationLocations_InvokesCommandHandler(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] MemberNotificationSettingsController sut,
            Guid memberId,
            UpdateMemberNotificationSettingsApiRequest request,
            CancellationToken cancellationToken)
        {
            // Act
            var result = await sut.PostMemberNotificationSettings(memberId, request, cancellationToken);

            // Assert
            mediatorMock.Verify(m => m.Send(It.Is<UpdateMemberNotificationSettingsCommand>(cmd =>
                cmd.MemberId == memberId &&
                cmd.ReceiveNotifications == request.ReceiveNotifications &&
                cmd.EventTypes.All(e => request.EventTypes.Any(re =>
                        re.ReceiveNotifications == e.ReceiveNotifications &&
                        re.EventType == e.EventType
                    )) &&
                cmd.Locations.All(l => request.Locations.Any(rl =>
                    rl.Name == l.Name &&
                    rl.Radius == l.Radius &&
                    rl.Latitude == l.Latitude &&
                    rl.Longitude == l.Longitude))), It.IsAny<CancellationToken>()), Times.Once);

            result.Should().BeOfType<OkResult>();
        }

        [Test, MoqAutoData]
        public async Task PostMemberNotificationLocations_ReturnsOkResponse(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] MemberNotificationSettingsController sut,
            Guid memberId,
            UpdateMemberNotificationSettingsApiRequest request,
            CancellationToken cancellationToken)
        {
            // Arrange
            mediatorMock.Setup(m => m.Send(It.IsAny<UpdateMemberNotificationSettingsCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await sut.PostMemberNotificationSettings(memberId, request, cancellationToken);

            // Assert
            result.Should().BeOfType<OkResult>();
        }
    }
}
