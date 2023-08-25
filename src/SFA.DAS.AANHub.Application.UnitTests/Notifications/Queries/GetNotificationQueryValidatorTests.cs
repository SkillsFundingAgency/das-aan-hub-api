﻿using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AANHub.Application.Employers.Queries;
using SFA.DAS.AANHub.Application.Notifications.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Notifications.Queries;
public class GetNotificationQueryValidatorTests
{

    [Test]
    [RecursiveMoqAutoData]
    public async Task ValidateCalendarId_Missing_FailsValidation(Member member)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(member.Id))
                                 .ReturnsAsync(member);

        var query = new GetCalendarEventByIdQuery(Guid.Empty, member.Id);

        var sut = new GetCalendarEventByIdQueryValidator(membersReadRepositoryMock.Object);

        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(c => c.CalendarEventId)
              .WithErrorMessage(GetCalendarEventByIdQueryValidator.CalendarEventIdMissingMessage);
    }

    [Test, RecursiveMoqInlineAutoData("93231c8f-7645-48c8-90e7-316f3f15c0ed")]
    public async Task Validate_NotificationIdIsValid(
        string notificationId,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        Member member)
    {
        var id = Guid.Parse(notificationId);
        member.Status = MembershipStatus.Live;
        membersReadRepository.Setup(m => m.GetMember(member.Id)).ReturnsAsync(member);

        var query = new GetNotificationQuery(id, member.Id);
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
        string notificationId,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        Member member)
    {
        var id = Guid.Parse(notificationId);
        membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);

        var sut = new GetNotificationQueryValidator(membersReadRepository.Object);
        var result = await sut.TestValidateAsync(new GetNotificationQuery(id, member.Id));

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(m => m.NotificationId).WithErrorMessage("NotificationId must have a value");
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
                .WithErrorMessage("Could not find a valid active Member ID matching the X-RequestedByMemberId header");
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
            result.ShouldHaveValidationErrorFor(m => m.RequestedByMemberId).WithErrorMessage("X-RequestedByMemberId header is empty");
        }
    }
}