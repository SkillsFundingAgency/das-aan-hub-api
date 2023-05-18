﻿using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Queries;
public class GetCalendarEventsQueryValidatorTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task ValidateCalendarId_Missing_FailsValidation(Member member)
    {

        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(member.Id))
            .ReturnsAsync(member);

        var query = new GetCalendarEventsQuery(member.Id, 1);
        var calendarEvents = (List<CalendarEventModel>)null!;
        var calendarEventsReadRepositoryMock = new Mock<ICalendarEventsReadRepository>();

        calendarEventsReadRepositoryMock.Setup(a => a.GetCalendarEvents(member.Id))!.ReturnsAsync(calendarEvents);
        var sut = new GetCalendarEventsQueryValidator(membersReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(c => c.RequestedByMemberId);
    }

    [TestCase(Domain.Common.Constants.MembershipStatus.Pending)]
    [TestCase(Domain.Common.Constants.MembershipStatus.Deleted)]
    [TestCase(Domain.Common.Constants.MembershipStatus.Withdrawn)]
    [TestCase(Domain.Common.Constants.MembershipStatus.Cancelled)]
    public async Task ValidateCalendarId_Inactive_FailsValidation(string inactiveMembershipStatus)
    {
        var inactiveGuid = Guid.NewGuid();

        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(inactiveGuid))
            .ReturnsAsync(new Member() { Status = inactiveMembershipStatus });

        var query = new GetCalendarEventsQuery(inactiveGuid, 1);

        var sut = new GetCalendarEventsQueryValidator(membersReadRepositoryMock.Object);

        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(x => x.RequestedByMemberId);
    }
}
