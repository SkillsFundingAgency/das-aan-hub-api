using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Employers.Queries;
using SFA.DAS.AANHub.Application.Notifications.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Notifications.Queries;
public class GetNotificationQueryValidatorTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Validate_NotificationIdIsValid(
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        Member member)
    {
        member.Status = MembershipStatus.Live;
        membersReadRepository.Setup(m => m.GetMember(member.Id)).ReturnsAsync(member);

        var query = new GetNotificationQuery(Guid.NewGuid(), member.Id);
        var sut = new GetNotificationQueryValidator(membersReadRepository.Object);
        var result = await sut.TestValidateAsync(query);
        var errors = result.Errors.ToList();

        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.ShouldNotHaveValidationErrorFor(c => c.NotificationId);
        }
    }

    [Test]
    [RecursiveMoqInlineAutoData("00000000-0000-0000-0000-000000000000")]
    public async Task Validate_NotificationIdIsNull(
        string mockNotificationId,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        Member member)
    {
        var notificationId = Guid.Parse(mockNotificationId);
        membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);

        var sut = new GetNotificationQueryValidator(membersReadRepository.Object);
        var result = await sut.TestValidateAsync(new GetNotificationQuery(notificationId, member.Id));

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(m => m.NotificationId).WithErrorMessage(GetNotificationQueryValidator.NotificationIdIsRequired);
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Validate_MemberIdIsValid(
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        GetMemberQuery query,
        Member member)
    {
        membersReadRepository.Setup(m => m.GetMember(member.Id)).ReturnsAsync(member);

        var sut = new GetMemberQueryValidator();
        var result = await sut.TestValidateAsync(query);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.ShouldNotHaveValidationErrorFor(m => m.UserRef);
        }
    }

    [Test, MoqAutoData]
    public async Task Validate_MemberIdIsNotFound(
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        GetNotificationQuery query)
    {
        membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>())).ReturnsAsync(() => null);

        var sut = new GetNotificationQueryValidator(membersReadRepository.Object);
        var result = await sut.TestValidateAsync(query);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(m => m.RequestedByMemberId)
                .WithErrorMessage(RequestedByMemberIdValidator.RequestedByMemberIdNotFoundMessage);
        }
    }

    [Test]
    [RecursiveMoqInlineAutoData("00000000-0000-0000-0000-000000000000")]
    public async Task Validate_MemberIdIsNull(
        string memberId,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository)
    {
        var id = Guid.Parse(memberId);

        var sut = new GetNotificationQueryValidator(membersReadRepository.Object);
        var result = await sut.TestValidateAsync(new GetNotificationQuery(It.IsAny<Guid>(), id));

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(m => m.RequestedByMemberId).WithErrorMessage(RequestedByMemberIdValidator.RequestedByMemberHeaderEmptyErrorMessage);
        }
    }
}
