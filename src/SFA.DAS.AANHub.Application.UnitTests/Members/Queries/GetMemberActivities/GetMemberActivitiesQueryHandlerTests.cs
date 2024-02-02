using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Queries.GetMemberActivities;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Application.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Queries.GetMemberActivities;
public class GetMemberActivitiesQueryHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_ShouldReturnGetMemberActivitiesQueryResult(
        [Frozen] Mock<IAttendancesReadRepository> attendancesReadRepository,
        [Frozen] Mock<IAuditReadRepository> auditReadRepository,
        GetMemberActivitiesQueryHandler sut,
        Audit audit,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        // Arrange
        List<Attendance> attendances = new List<Attendance>()
        {
            new Attendance(){Id=Guid.NewGuid(),CalendarEventId=Guid.NewGuid(),AddedDate = DateTime.UtcNow,MemberId=memberId,IsAttending=true,CalendarEvent = new CalendarEvent(){ Title= "Event Title 1",Urn=null,StartDate=DateTime.UtcNow.AddDays(-5).Date} },
            new Attendance(){Id=Guid.NewGuid(),CalendarEventId=Guid.NewGuid(),AddedDate = DateTime.UtcNow,MemberId=memberId,IsAttending=true,CalendarEvent = new CalendarEvent(){ Title= "Event Title 2",Urn=null,StartDate=DateTime.UtcNow.AddDays(-2).Date}},
            new Attendance(){Id=Guid.NewGuid(),CalendarEventId=Guid.NewGuid(),AddedDate = DateTime.UtcNow,MemberId=memberId,IsAttending=true,CalendarEvent = new CalendarEvent(){ Title= "Event Title 3",Urn=null,StartDate=DateTime.UtcNow.Date}},
            new Attendance(){Id=Guid.NewGuid(),CalendarEventId=Guid.NewGuid(),AddedDate = DateTime.UtcNow,MemberId=memberId,IsAttending=true,CalendarEvent = new CalendarEvent(){ Title= "Event Title 4",Urn=null,StartDate=DateTime.UtcNow.AddDays(2).Date}},
            new Attendance(){Id=Guid.NewGuid(),CalendarEventId=Guid.NewGuid(),AddedDate = DateTime.UtcNow,MemberId=memberId,IsAttending=true,CalendarEvent = new CalendarEvent(){ Title= "Event Title 5",Urn=null,StartDate=DateTime.UtcNow.AddDays(5).Date}},
        };

        attendancesReadRepository.Setup(a => a.GetAttendances(memberId, DateTime.UtcNow.Date.AddMonths(-1 * RangeDuration.EventsRangePeriod), DateTime.UtcNow.Date.AddMonths(RangeDuration.EventsRangePeriod), cancellationToken)).ReturnsAsync(attendances);
        auditReadRepository.Setup(a => a.GetLastAttendanceAuditByMemberId(memberId, cancellationToken)).ReturnsAsync(audit);

        // Act
        var result = await sut.Handle(new GetMemberActivitiesQuery(memberId), cancellationToken);

        // Assert
        Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.InstanceOf<GetMemberActivitiesQueryResult>());
                Assert.That(result.Result.LastSignedUpDate, Is.EqualTo(audit.AuditTime));
                Assert.That(result.Result.EventsPlanned, Is.Not.Null);
                Assert.That(result.Result.EventsPlanned.Events, Is.Not.Null);
                Assert.That(result.Result.EventsPlanned.Events.Count, Is.EqualTo(2));
                Assert.That(result.Result.EventsPlanned.EventsDateRange, Is.Not.Null);
                Assert.That(result.Result.EventsPlanned.EventsDateRange.FromDate, Is.EqualTo(DateTime.UtcNow.Date.AddDays(1)));
                Assert.That(result.Result.EventsPlanned.EventsDateRange.ToDate, Is.EqualTo(DateTime.UtcNow.Date.AddMonths(RangeDuration.EventsRangePeriod)));
                Assert.That(result.Result.EventsAttended, Is.Not.Null);
                Assert.That(result.Result.EventsAttended.Events, Is.Not.Null);
                Assert.That(result.Result.EventsAttended.Events.Count, Is.EqualTo(3));
                Assert.That(result.Result.EventsAttended.EventsDateRange, Is.Not.Null);
                Assert.That(result.Result.EventsAttended.EventsDateRange.FromDate, Is.EqualTo(DateTime.UtcNow.Date.AddMonths(-1 * RangeDuration.EventsRangePeriod)));
                Assert.That(result.Result.EventsAttended.EventsDateRange.ToDate, Is.EqualTo(DateTime.UtcNow.Date));
            });
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_MemberWithNoActivity(
        [Frozen] Mock<IAttendancesReadRepository> attendancesReadRepository,
        [Frozen] Mock<IAuditReadRepository> auditReadRepository,
        GetMemberActivitiesQueryHandler sut,
        Audit audit,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        // Arrange
        List<Attendance> attendances = new List<Attendance>();
        attendancesReadRepository.Setup(a => a.GetAttendances(memberId, DateTime.UtcNow.AddMonths(-1 * RangeDuration.EventsRangePeriod).Date, DateTime.UtcNow.AddMonths(RangeDuration.EventsRangePeriod).Date, cancellationToken)).ReturnsAsync(attendances);
        auditReadRepository.Setup(a => a.GetLastAttendanceAuditByMemberId(memberId, cancellationToken)).ReturnsAsync(audit);

        // Act
        var result = await sut.Handle(new GetMemberActivitiesQuery(memberId), cancellationToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Result.EventsPlanned.Events, Is.Not.Null);
            Assert.That(result.Result.EventsPlanned.Events.Count, Is.EqualTo(0));
            Assert.That(result.Result.EventsPlanned.EventsDateRange.FromDate, Is.EqualTo(DateTime.UtcNow.Date.AddDays(1)));
            Assert.That(result.Result.EventsPlanned.EventsDateRange.ToDate, Is.EqualTo(DateTime.UtcNow.Date.AddMonths(RangeDuration.EventsRangePeriod)));
            Assert.That(result.Result.EventsAttended.Events, Is.Not.Null);
            Assert.That(result.Result.EventsAttended.Events.Count, Is.EqualTo(0));
            Assert.That(result.Result.EventsAttended.EventsDateRange.FromDate, Is.EqualTo(DateTime.UtcNow.Date.AddMonths(-1 * RangeDuration.EventsRangePeriod)));
            Assert.That(result.Result.EventsAttended.EventsDateRange.ToDate, Is.EqualTo(DateTime.UtcNow.Date));
        });
    }
}
