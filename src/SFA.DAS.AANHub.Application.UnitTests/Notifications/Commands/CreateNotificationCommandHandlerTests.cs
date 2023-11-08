using System.Text.Json;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Notifications.Commands;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Notifications.Commands;
public class CreateNotificationCommandHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_CreateNotification_ReturnSuccess(
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<INotificationTemplateReadRepository> notificationTemplateReadRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationsWriteRepository,
        [Frozen] Mock<IAanDataContext> aanDataContext,
        Member requestedByMember,
        Member member,
        NotificationTemplate notificationTemplate,
        [Greedy] CreateNotificationCommandHandler sut,
        CreateNotificationCommand command)
    {
        requestedByMember.Id = command.RequestedByMemberId;
        member.Id = command.MemberId;
        notificationTemplate.Id = command.NotificationTemplateId;
        membersReadRepository.Setup(m => m.GetMember(command.RequestedByMemberId)).ReturnsAsync(requestedByMember);
        membersReadRepository.Setup(m => m.GetMember(command.MemberId)).ReturnsAsync(member);
        notificationTemplateReadRepository.Setup(nt => nt.Get(command.NotificationTemplateId, It.IsAny<CancellationToken>())).ReturnsAsync(notificationTemplate);

        var response = await sut.Handle(command, It.IsAny<CancellationToken>());
        var mockToken = new MemberToMemberContactEmailTemplate(member.FullName, requestedByMember.FullName, requestedByMember.FirstName, requestedByMember.Email);
        var mockTokenSerialised = JsonSerializer.Serialize(mockToken);

        using (new AssertionScope())
        {
            response.Result.NotificationId.Should().NotBeEmpty();
            membersReadRepository.Verify(m => m.GetMember(requestedByMember.Id), Times.Once);
            membersReadRepository.Verify(m => m.GetMember(member.Id), Times.Once);
            notificationTemplateReadRepository.Verify(nt => nt.Get(notificationTemplate.Id, It.IsAny<CancellationToken>()), Times.Once);
            notificationsWriteRepository.Verify(n => n.Create(It.Is<Notification>(n => n.Tokens.Equals(mockTokenSerialised) && n.IsSystem.Equals(false) && n.CreatedBy.Equals(requestedByMember.Id) && n.MemberId.Equals(member.Id) && n.ReferenceId!.Equals(requestedByMember.Id.ToString()))), Times.Once);
            //notificationsWriteRepository.Verify(n => n.Create(It.Is<Notification>(n => n.IsSystem.Equals(false))));
            //notificationsWriteRepository.Verify(n => n.Create(It.Is<Notification>(n => n.CreatedBy.Equals(requestedByMember))));
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        };
    }
}
