using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.DeleteCalendarEvent;
using SFA.DAS.AANHub.Application.Extensions;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;
using System.Text.Json;

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
        CalendarEvent calendarEvent,
        Guid memberId)
    {
        calendarEventsWriteRepository.Setup(x => x.GetCalendarEvent(It.IsAny<Guid>())).ReturnsAsync(calendarEvent);

        var command = new DeleteCalendarEventCommand(calendarEvent.Id, memberId);
        var singleMemberId = members.First().Id;

        var attendances = new List<Attendance>
        {
            new()
            {
                CalendarEventId = calendarEvent.Id,
                MemberId = singleMemberId
            }
        };

        attendancesWriteRepository.Setup(x => x.GetAttendancesByEventId(calendarEvent.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(attendances);

        membersReadRepository.Setup(x => x.GetMembers(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(members);

        await sut.Handle(command, new CancellationToken());


        var startDate = calendarEvent.StartDate.UtcToLocalTime();

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
            attendancesWriteRepository.Verify(a => a.GetAttendancesByEventId(calendarEvent.Id, It.IsAny<CancellationToken>()), Times.Once);
            membersReadRepository.Verify(m => m.GetMembers(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
            notificationsWriteRepository.Verify(n => n.Create(It.Is<Notification>(
                q => q.MemberId == singleMemberId
                     && q.TemplateName == Constants.NotificationTemplateNames.AANAdminEventCancel
                     && q.CreatedBy == command.RequestedByMemberId
                     && q.IsSystem == true
                     && q.ReferenceId == calendarEvent.Id.ToString()
                     && q.Tokens == expectedTokens)), Times.Once);
            auditWriteRepository.Verify(a => a.Create(It.IsAny<Audit>()), Times.Once);
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}