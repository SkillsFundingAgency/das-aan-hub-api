using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Notifications.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Notifications.Queries;
public class GetNotificationQueryHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_MemberValidAndNotificationFound_ReturnNotification(
        [Frozen] Mock<INotificationsReadRepository> notificationsReadRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        GetNotificationQueryHandler sut,
        GetNotificationQuery query,
        Member member,
        Notification notification)
    {
        notification.Id = query.NotificationId;
        member.Id = query.RequestedByMemberId;
        notification.MemberId = member.Id;
        membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        notificationsReadRepository.Setup(n => n.GetNotificationById(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(notification);

        var result = await sut.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(notification, n => n.ExcludingMissingMembers());
            membersReadRepository.Verify(m => m.GetMember(It.IsAny<Guid>()), Times.Once());
            notificationsReadRepository.Verify(n => n.GetNotificationById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_MemberIdDoesNotMatch_Return_Null(
        [Frozen] Mock<INotificationsReadRepository> notificationsReadRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        GetNotificationQueryHandler sut,
        GetNotificationQuery query,
        Member member,
        Notification notification)
    {
        notification.Id = query.NotificationId;
        membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        notificationsReadRepository.Setup(n => n.GetNotificationById(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(notification);

        var result = await sut.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Result.Should().BeNull();
            membersReadRepository.Verify(m => m.GetMember(It.IsAny<Guid>()), Times.Never());
            notificationsReadRepository.Verify(n => n.GetNotificationById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_NotificationNotFound_ReturnNull(
        [Frozen] Mock<INotificationsReadRepository> notificationsReadRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        GetNotificationQueryHandler sut,
        GetNotificationQuery query,
        Member member)
    {
        membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        notificationsReadRepository.Setup(n => n.GetNotificationById(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(() => null);

        var result = await sut.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Result.Should().BeNull();
            membersReadRepository.Verify(m => m.GetMember(It.IsAny<Guid>()), Times.Never());
            notificationsReadRepository.Verify(n => n.GetNotificationById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_NotificationAndMemberFound_AndMemberIsEmployer_PopulateEmployerAccountId(
        [Frozen] Mock<INotificationsReadRepository> notificationsReadRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<IEmployersReadRepository> employersReadRepository,
        GetNotificationQueryHandler sut,
        GetNotificationQuery query,
        Employer employer,
        Member member,
        Notification notification)
    {
        notification.Id = query.NotificationId;
        member.Id = query.RequestedByMemberId;
        notification.MemberId = member.Id;
        member.UserType = "Employer";
        employer.MemberId = member.Id;
        membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        notificationsReadRepository.Setup(n => n.GetNotificationById(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(notification);
        employersReadRepository.Setup(e => e.GetEmployerByMemberId(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(employer);

        var result = await sut.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Result.EmployerAccountId.Should().NotBeNull();
            result.Result.EmployerAccountId.Should().Be(employer.AccountId);
            result.Should().BeEquivalentTo(notification, n => n.ExcludingMissingMembers());
            membersReadRepository.Verify(m => m.GetMember(It.IsAny<Guid>()), Times.Once());
            employersReadRepository.Verify(e => e.GetEmployerByMemberId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once());
            notificationsReadRepository.Verify(n => n.GetNotificationById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_NotificationAndMemberFound_AndMemberIsApprentice_DoNotPopulateEmployerAccountId(
        [Frozen] Mock<INotificationsReadRepository> notificationsReadRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<IEmployersReadRepository> employersReadRepository,
        GetNotificationQueryHandler sut,
        GetNotificationQuery query,
        Member member,
        Notification notification)
    {
        notification.Id = query.NotificationId;
        member.Id = query.RequestedByMemberId;
        notification.MemberId = member.Id;
        member.UserType = "Apprentice";
        membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        notificationsReadRepository.Setup(n => n.GetNotificationById(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(notification);

        var result = await sut.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Result.EmployerAccountId.Should().BeNull();
            result.Should().BeEquivalentTo(notification, n => n.ExcludingMissingMembers());
            membersReadRepository.Verify(m => m.GetMember(It.IsAny<Guid>()), Times.Once());
            employersReadRepository.Verify(e => e.GetEmployerByMemberId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never());
            notificationsReadRepository.Verify(n => n.GetNotificationById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
