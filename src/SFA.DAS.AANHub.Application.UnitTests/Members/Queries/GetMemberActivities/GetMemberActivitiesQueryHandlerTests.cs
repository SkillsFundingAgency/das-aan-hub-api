﻿using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Queries.GetMemberActivities;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Queries.GetMemberActivities;
public class GetMemberActivitiesQueryHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_ShouldReturnGetMemberActivitiesResult(
        [Frozen] Mock<IAttendancesReadRepository> attendancesReadRepository,
        [Frozen] Mock<IAuditReadRepository> auditReadRepository,
        GetMemberActivitiesQueryHandler sut,
        Audit audit,
        Guid memberId)
    {
        // Arrange
        List<Attendance> attendances = new List<Attendance>()
        {
            new Attendance(){Id=Guid.NewGuid(),CalendarEventId=Guid.NewGuid(),AddedDate = DateTime.UtcNow.AddDays(-5),MemberId=memberId,IsAttending=true,CalendarEvent = new CalendarEvent(){ Title= "Event Title 1",Urn=null} },
            new Attendance(){Id=Guid.NewGuid(),CalendarEventId=Guid.NewGuid(),AddedDate = DateTime.UtcNow.AddDays(-2),MemberId=memberId,IsAttending=true,CalendarEvent = new CalendarEvent(){ Title= "Event Title 2",Urn=null}},
            new Attendance(){Id=Guid.NewGuid(),CalendarEventId=Guid.NewGuid(),AddedDate = DateTime.UtcNow,MemberId=memberId,IsAttending=true,CalendarEvent = new CalendarEvent(){ Title= "Event Title 3",Urn=null}},
            new Attendance(){Id=Guid.NewGuid(),CalendarEventId=Guid.NewGuid(),AddedDate = DateTime.UtcNow.AddDays(2),MemberId=memberId,IsAttending=true,CalendarEvent = new CalendarEvent(){ Title= "Event Title 4",Urn=null}},
            new Attendance(){Id=Guid.NewGuid(),CalendarEventId=Guid.NewGuid(),AddedDate = DateTime.UtcNow.AddDays(5),MemberId=memberId,IsAttending=true,CalendarEvent = new CalendarEvent(){ Title= "Event Title 5",Urn=null}},
        };

        attendancesReadRepository.Setup(a => a.GetAttendances(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(attendances);
        auditReadRepository.Setup(a => a.GetLastAttendanceAuditByMemberId(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(audit);

        // Act
        var result = await sut.Handle(new GetMemberActivitiesQuery(memberId), new CancellationToken());

        // Assert
        Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.InstanceOf<GetMemberActivitiesResult>());
                Assert.That(result.Result.LastSignedUpDate, Is.EqualTo(audit.AuditTime));
                Assert.That(result.Result.EventsPlanned, Is.Not.Null);
                Assert.That(result.Result.EventsPlanned.Events, Is.Not.Null);
                Assert.That(result.Result.EventsPlanned.Events.Count, Is.EqualTo(2));
                Assert.That(result.Result.EventsPlanned.EventsDateRange, Is.Not.Null);
                Assert.That(result.Result.EventsAttended, Is.Not.Null);
                Assert.That(result.Result.EventsAttended.Events, Is.Not.Null);
                Assert.That(result.Result.EventsAttended.Events.Count, Is.EqualTo(3));
                Assert.That(result.Result.EventsAttended.EventsDateRange, Is.Not.Null);
            });
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_MemberWithNoActivity(
        [Frozen] Mock<IAttendancesReadRepository> attendancesReadRepository,
        [Frozen] Mock<IAuditReadRepository> auditReadRepository,
        GetMemberActivitiesQueryHandler sut,
        Audit audit,
        Guid memberId)
    {
        // Arrange
        List<Attendance> attendances = new List<Attendance>();
        attendancesReadRepository.Setup(a => a.GetAttendances(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(attendances);
        auditReadRepository.Setup(a => a.GetLastAttendanceAuditByMemberId(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(audit);

        // Act
        var result = await sut.Handle(new GetMemberActivitiesQuery(memberId), new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Result.EventsPlanned.Events, Is.Not.Null);
            Assert.That(result.Result.EventsPlanned.Events.Count, Is.EqualTo(0));
            Assert.That(result.Result.EventsAttended.Events, Is.Not.Null);
            Assert.That(result.Result.EventsAttended.Events.Count, Is.EqualTo(0));
        });
    }
}