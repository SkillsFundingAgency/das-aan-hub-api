using AutoFixture.NUnit3;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Notifications.Commands;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Notifications.Commands;
public class CreateNotificationCommandValidatorTests
{
    [Test]
    [RecursiveMoqInlineAutoData(UserType.Apprentice)]
    [RecursiveMoqInlineAutoData(UserType.Employer)]
    public async Task ValidateCreateNotification_IsValid(
        UserType userType,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<INotificationTemplateReadRepository> notificationTemplateReadRepository,
        Member member,
        Member requestedByMember,
        NotificationTemplate notificationTemplate)
    {
        member.UserType = userType;
        member.Status = MembershipStatus.Live;
        requestedByMember.Status = MembershipStatus.Live;
        var command = new CreateNotificationCommand() { MemberId = member.Id, RequestedByMemberId = requestedByMember.Id, NotificationTemplateId = notificationTemplate.Id };
        membersReadRepository.Setup(m => m.GetMember(command.MemberId)).ReturnsAsync(member);
        membersReadRepository.Setup(m => m.GetMember(command.RequestedByMemberId)).ReturnsAsync(requestedByMember);
        notificationTemplateReadRepository.Setup(nt => nt.Get(command.NotificationTemplateId, It.IsAny<CancellationToken>())).ReturnsAsync(notificationTemplate);

        var sut = new CreateNotificationCommandValidator(membersReadRepository.Object, notificationTemplateReadRepository.Object);
        var result = await sut.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test, RecursiveMoqAutoData]
    public async Task ValidateNotificationTemplateId_DoesNotExist_FailsValidation(Member member)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(member.Id))
                         .ReturnsAsync(member);
        var notificationTemplateReadRepositoryMock = new Mock<INotificationTemplateReadRepository>();
        notificationTemplateReadRepositoryMock.Setup(nt => nt.Get(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

        var command = new CreateNotificationCommand() { RequestedByMemberId = Guid.NewGuid(), MemberId = Guid.NewGuid(), NotificationTemplateId = It.IsAny<int>() };
        var sut = new CreateNotificationCommandValidator(membersReadRepositoryMock.Object, notificationTemplateReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(nt => nt.NotificationTemplateId).WithErrorMessage(CreateNotificationCommandValidator.NotificationTemplateNotFound);
    }

    [Test, RecursiveMoqAutoData]
    public async Task ValidateNotificationTemplateId_Empty_FailsValidation(Member member)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(member.Id)).ReturnsAsync(member);
        var notificationTemplateReadRepositoryMock = new Mock<INotificationTemplateReadRepository>();
        notificationTemplateReadRepositoryMock.Setup(nt => nt.Get(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

        var command = new CreateNotificationCommand() { RequestedByMemberId = Guid.NewGuid(), MemberId = Guid.NewGuid(), NotificationTemplateId = 00000000 };
        var sut = new CreateNotificationCommandValidator(membersReadRepositoryMock.Object, notificationTemplateReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(nt => nt.NotificationTemplateId).WithErrorMessage(CreateNotificationCommandValidator.NotificationTemplateIdRequired);
    }

    [Test, MoqAutoData]
    public async Task ValidateMemberId_Empty_FailsValidation(CreateNotificationCommand mockCommand)
    {
        mockCommand.MemberId = Guid.Empty;
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        var notificationTemplateReadRepositoryMock = new Mock<INotificationTemplateReadRepository>();

        var sut = new CreateNotificationCommandValidator(membersReadRepositoryMock.Object, notificationTemplateReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(mockCommand);

        result.ShouldHaveValidationErrorFor(nt => nt.MemberId).WithErrorMessage(MemberIdValidator.MemberIdEmptyErrorMessage);
    }

    [Test, MoqAutoData]
    public async Task ValidateMemberId_WhenNoMemberReturned_FailsValidation(CreateNotificationCommand mockCommand)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(It.IsAny<Guid>())).ReturnsAsync(() => null);
        var notificationTemplateReadRepositoryMock = new Mock<INotificationTemplateReadRepository>();

        var sut = new CreateNotificationCommandValidator(membersReadRepositoryMock.Object, notificationTemplateReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(mockCommand);

        result.ShouldHaveValidationErrorFor(nt => nt.MemberId).WithErrorMessage(MemberIdValidator.MemberIdMustBeLive);
    }

    [Test, MoqAutoData]
    public async Task ValidateRequestedByMemberId_Empty_FailsValidation(CreateNotificationCommand mockCommand)
    {
        mockCommand.RequestedByMemberId = Guid.Empty;
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        var notificationTemplateReadRepositoryMock = new Mock<INotificationTemplateReadRepository>();

        var sut = new CreateNotificationCommandValidator(membersReadRepositoryMock.Object, notificationTemplateReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(mockCommand);

        result.ShouldHaveValidationErrorFor(nt => nt.RequestedByMemberId).WithErrorMessage(RequestedByMemberIdValidator.RequestedByMemberHeaderEmptyErrorMessage);
    }

    [Test, MoqAutoData]
    public async Task ValidateRequestedByMemberId_WhenNoMemberReturned_FailsValidation(CreateNotificationCommand mockCommand)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(mockCommand.RequestedByMemberId)).ReturnsAsync(() => null);
        var notificationTemplateReadRepositoryMock = new Mock<INotificationTemplateReadRepository>();

        var sut = new CreateNotificationCommandValidator(membersReadRepositoryMock.Object, notificationTemplateReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(mockCommand);

        result.ShouldHaveValidationErrorFor(nt => nt.RequestedByMemberId).WithErrorMessage(RequestedByMemberIdValidator.RequestedByMemberIdNotFoundMessage);
    }
}
