using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;
using System.Text.Json;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.PutCalendarEvent;

public class PutCalendarEventCommandHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_PutCalendarEvent(
        [Frozen] Mock<IAanDataContext> aanDataContext,
        [Frozen] Mock<ICalendarEventsWriteRepository> calendarEventsWriteRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationsWriteRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] PutCalendarEventModel model,
        [Frozen] List<Member> members,
        CalendarEvent calendarEvent)
    {
        var command = (PutCalendarEventCommand)model;
        command.CalendarEventId = calendarEvent.Id;
        command.SendUpdateEventNotification = true;
        var singleMemberId = members.First().Id;

        var attendances = new List<Attendance>
        {
            new()
            {
                CalendarEventId = calendarEvent.Id,
                MemberId = singleMemberId
            }
        };

        calendarEventsWriteRepository.Setup(x => x.GetCalendarEvent(calendarEvent.Id)).ReturnsAsync(calendarEvent);

        var attendancesReadRepository = new Mock<IAttendancesReadRepository>();

        attendancesReadRepository.Setup(x => x.GetAttendancesByEventId(calendarEvent.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(attendances);

        membersReadRepository.Setup(x => x.GetMembers(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(members);

        var sut = new PutCalendarEventCommandHandler(calendarEventsWriteRepository.Object, auditWriteRepository.Object, attendancesReadRepository.Object, notificationsWriteRepository.Object, membersReadRepository.Object, aanDataContext.Object);

        await sut.Handle(command, new CancellationToken());

        var localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        var startDate = TimeZoneInfo.ConvertTimeFromUtc(calendarEvent.StartDate, localTimeZone);

        var date = startDate.ToString("dd/MM/yyyy");
        var time = startDate.ToString("HH:mm");

        var tokens = new Dictionary<string, string>
        {
            { "contact", members.First().FullName },
            { "eventname", calendarEvent.Title },
            { "date", date },
            { "time", time }
        };

        var expectedTokens = JsonSerializer.Serialize(tokens);

        using (new AssertionScope())
        {
            calendarEventsWriteRepository.Verify(c => c.GetCalendarEvent(calendarEvent.Id), Times.Once);
            attendancesReadRepository.Verify(a => a.GetAttendancesByEventId(calendarEvent.Id, It.IsAny<CancellationToken>()), Times.Once);
            membersReadRepository.Verify(m => m.GetMembers(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
            notificationsWriteRepository.Verify(n => n.Create(It.Is<Notification>(
                q => q.MemberId == singleMemberId
                     && q.TemplateName == Constants.NotificationTemplateNames.AANAdminEventUpdate
                     && q.CreatedBy == command.AdminMemberId
                     && q.IsSystem == true
                     && q.ReferenceId == calendarEvent.Id.ToString()
                     && q.Tokens == expectedTokens)), Times.Exactly(attendances.Count));
            auditWriteRepository.Verify(a => a.Create(It.IsAny<Audit>()), Times.Once);
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_PutCalendarEvent_SendNotificationFalse(
        [Frozen] Mock<IAanDataContext> aanDataContext,
        [Frozen] Mock<ICalendarEventsWriteRepository> calendarEventsWriteRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationsWriteRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] PutCalendarEventModel model,
        Guid calendarEventId)
    {
        var command = (PutCalendarEventCommand)model;
        command.CalendarEventId = calendarEventId;
        command.SendUpdateEventNotification = false;

        var calendarEvent = new CalendarEvent { Id = calendarEventId };

        calendarEventsWriteRepository.Setup(x => x.GetCalendarEvent(calendarEventId)).ReturnsAsync(calendarEvent);

        var attendancesReadRepository = new Mock<IAttendancesReadRepository>();

        var sut = new PutCalendarEventCommandHandler(calendarEventsWriteRepository.Object, auditWriteRepository.Object, attendancesReadRepository.Object, notificationsWriteRepository.Object, membersReadRepository.Object, aanDataContext.Object);

        await sut.Handle(command, new CancellationToken());

        using (new AssertionScope())
        {
            calendarEventsWriteRepository.Verify(c => c.GetCalendarEvent(calendarEventId), Times.Once);
            attendancesReadRepository.Verify(a => a.GetAttendancesByEventId(calendarEventId, It.IsAny<CancellationToken>()), Times.Never);
            membersReadRepository.Verify(m => m.GetMembers(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Never);
            notificationsWriteRepository.Verify(n => n.Create(It.IsAny<Notification>()), Times.Never);
            auditWriteRepository.Verify(a => a.Create(It.IsAny<Audit>()), Times.Once);
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
