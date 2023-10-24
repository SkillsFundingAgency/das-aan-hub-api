using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.DeleteCalendarEvent;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.DeleteCalendarEvent;

public class DeleteCalendarEventCommandHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_DeleteCalendarEvent(
        [Frozen] Mock<IAanDataContext> aanDataContext,
        [Frozen] Mock<ICalendarEventsWriteRepository> calendarEventsWriteRepository,
        [Frozen] Mock<IAttendancesWriteRepository> attendancesWriteRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationsWriteRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] DeleteCalendarEventCommandHandler sut,
        [Frozen] List<Member> members,
        Guid calendarEventId,
        Guid memberId)
    {
        var command = new DeleteCalendarEventCommand(calendarEventId, memberId);
        var singleMemberId = members.First().Id;

        var attendances = new List<Attendance>
        {
            new()
            {
                CalendarEventId = calendarEventId,
                MemberId = singleMemberId
            }
        };

        attendancesWriteRepository.Setup(x => x.GetAttendancesByEventId(calendarEventId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(attendances);

        membersReadRepository.Setup(x => x.GetMembers(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(members);

        await sut.Handle(command, new CancellationToken());

        using (new AssertionScope())
        {
            calendarEventsWriteRepository.Verify(c => c.GetCalendarEvent(calendarEventId), Times.Once);
            attendancesWriteRepository.Verify(a => a.GetAttendancesByEventId(calendarEventId, It.IsAny<CancellationToken>()), Times.Once);
            membersReadRepository.Verify(m => m.GetMembers(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
            notificationsWriteRepository.Verify(n => n.Create(It.IsAny<Notification>()), Times.Exactly(attendances.Count));
            auditWriteRepository.Verify(a => a.Create(It.IsAny<Audit>()), Times.Once);
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}