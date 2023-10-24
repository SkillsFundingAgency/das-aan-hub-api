using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Text.Json;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.DeleteCalendarEvent;
public class DeleteCalendarEventCommandHandler : IRequestHandler<DeleteCalendarEventCommand, ValidatedResponse<SuccessCommandResult>>
{
    private readonly IAanDataContext _aanDataContext;
    private readonly IMembersReadRepository _membersReadRepository;
    private readonly ICalendarEventsWriteRepository _calendarEventsWriteRepository;
    private readonly IAttendancesWriteRepository _attendancesWriteRepository;
    private readonly INotificationsWriteRepository _notificationsWriteRepository;
    private readonly IAuditWriteRepository _auditWriteRepository;

    public DeleteCalendarEventCommandHandler(IAanDataContext aanDataContext,
        IMembersReadRepository membersReadRepository,
        ICalendarEventsWriteRepository calendarEventsWriteRepository,
        IAttendancesWriteRepository attendancesWriteRepository,
        INotificationsWriteRepository notificationsWriteRepository,
        IAuditWriteRepository auditWriteRepository)
    {
        _aanDataContext = aanDataContext;
        _membersReadRepository = membersReadRepository;
        _calendarEventsWriteRepository = calendarEventsWriteRepository;
        _attendancesWriteRepository = attendancesWriteRepository;
        _notificationsWriteRepository = notificationsWriteRepository;
        _auditWriteRepository = auditWriteRepository;
    }
    public async Task<ValidatedResponse<SuccessCommandResult>> Handle(DeleteCalendarEventCommand command, CancellationToken cancellationToken)
    {
        var calendarEvent = await _calendarEventsWriteRepository.GetCalendarEvent(command.CalendarEventId);
        var calendarEventBefore = JsonSerializer.Serialize(calendarEvent);
        calendarEvent!.IsActive = false;
        calendarEvent.LastUpdatedDate = DateTime.UtcNow;
        var calendarEventAfter = JsonSerializer.Serialize(calendarEvent);

        var existingAttendances = await _attendancesWriteRepository.GetAttendancesByEventId(command.CalendarEventId, cancellationToken);

        if (existingAttendances.Any())
        {
            var members = await _membersReadRepository.GetMembers(existingAttendances.Select(x => x.MemberId).ToList(),
                cancellationToken);

            foreach (var attendance in existingAttendances)
            {
                attendance.IsAttending = false;

                var member = members.First(x => x.Id == attendance.MemberId);
                var templateName = Constants.NotificationTemplateNames.AANAdminEventCancel;
                var tokens = await GetTokens(calendarEvent, member);
                var notification = NotificationHelper.CreateNotification(Guid.NewGuid(), attendance.MemberId,
                    templateName, tokens, command.RequestedByMemberId, true, command.CalendarEventId.ToString());
                _notificationsWriteRepository.Create(notification);
            }
        }

        var audit = new Audit()
        {
            Action = "Cancelled",
            Before = calendarEventBefore,
            After = calendarEventAfter,
            ActionedBy = command.RequestedByMemberId,
            AuditTime = DateTime.UtcNow,
            Resource = nameof(CalendarEvent),
        };

        _auditWriteRepository.Create(audit);

        await _aanDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
    }

    private static Task<string> GetTokens(CalendarEvent calendarEvent, Member member)
    {
        var date = calendarEvent!.StartDate.ToString("dd/MM/yyyy");
        var time = calendarEvent!.StartDate.ToString("HH:mm");
        var fullname = member.FullName;
        var eventName = calendarEvent.Title;
        var emailAddress = member.Email;

        var tokens = new Dictionary<string, string>
        {
            { "email address", emailAddress },
            { "contact", fullname },
            { "eventname", eventName },
            { "date", date },
            { "time", time }
        };

        return Task.FromResult(JsonSerializer.Serialize(tokens));
    }


}
