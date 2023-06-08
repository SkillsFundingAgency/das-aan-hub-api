using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Queries;
public class GetCalendarEventsQueryValidatorTests
{
    [Test, RecursiveMoqAutoData]
    public async Task ValidateMemberId_NotActiveMemberId_FailsValidation(Member member, CancellationToken cancellationToken)
    {
        var startDate = DateTime.UtcNow;
        var toDate = DateTime.Today.AddYears(1);

        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(member.Id))
            .ReturnsAsync(member);

        var query = new GetCalendarEventsQuery(member.Id, startDate, toDate, new List<EventFormat>(), new List<int>(), new List<int>(), 1);
        var calendarEvents = (List<CalendarEventSummary>)null!;
        var calendarEventsReadRepositoryMock = new Mock<ICalendarEventsReadRepository>();


        calendarEventsReadRepositoryMock.Setup(a => a.GetCalendarEvents(new GetCalendarEventsOptions(member.Id, startDate, toDate, new List<EventFormat>(), new List<int>(), new List<int>(), 1), cancellationToken))!.ReturnsAsync(calendarEvents);
        var sut = new GetCalendarEventsQueryValidator(membersReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(query, cancellationToken: cancellationToken);

        result.ShouldHaveValidationErrorFor(c => c.RequestedByMemberId);
    }

    [TestCase(Domain.Common.Constants.MembershipStatus.Pending)]
    [TestCase(Domain.Common.Constants.MembershipStatus.Deleted)]
    [TestCase(Domain.Common.Constants.MembershipStatus.Withdrawn)]
    [TestCase(Domain.Common.Constants.MembershipStatus.Cancelled)]
    public async Task ValidateMemberId_MembershipStatusNotActive_FailsValidation(string inactiveMembershipStatus)
    {
        var inactiveGuid = Guid.NewGuid();

        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(inactiveGuid))
            .ReturnsAsync(new Member() { Status = inactiveMembershipStatus });

        var query = new GetCalendarEventsQuery(inactiveGuid, DateTime.Today, DateTime.Today, new List<EventFormat>(), new List<int>(), new List<int>(), 1);

        var sut = new GetCalendarEventsQueryValidator(membersReadRepositoryMock.Object);

        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(x => x.RequestedByMemberId);
    }
}