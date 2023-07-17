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
        var fromDate = DateTime.UtcNow;
        var toDate = DateTime.Today.AddYears(1);

        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(member.Id))
            .ReturnsAsync(member);

        var query = new GetCalendarEventsQuery
        {
            RequestedByMemberId = member.Id,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormats = new List<EventFormat>(),
            CalendarIds = new List<int>(),
            RegionIds = new List<int>(),
            Page = 1,
            PageSize = 5
        };

        var calendarEvents = (List<CalendarEventSummary>)null!;
        var calendarEventsReadRepositoryMock = new Mock<ICalendarEventsReadRepository>();

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(new GetCalendarEventsOptions
        {
            MemberId = member.Id,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormats = new List<EventFormat>(),
            CalendarIds = new List<int>(),
            RegionIds = new List<int>(),
            Page = 1,
            PageSize = 5
        }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(calendarEvents);
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

        var query = new GetCalendarEventsQuery
        {
            RequestedByMemberId = inactiveGuid,
            FromDate = DateTime.Today,
            ToDate = DateTime.Today,
            EventFormats = new List<EventFormat>(),
            CalendarIds = new List<int>(),
            RegionIds = new List<int>(),
            Keyword = string.Empty,
            Page = 1,
            PageSize = 5
        };

        var sut = new GetCalendarEventsQueryValidator(membersReadRepositoryMock.Object);

        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(x => x.RequestedByMemberId);
    }
}