using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Text.Json;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
public class PutCalendarEventCommandHandler : IRequestHandler<PutCalendarEventCommand, ValidatedResponse<SuccessCommandResult>>
{
    private readonly ICalendarEventsWriteRepository _calendarEventWriteRepository;
    private readonly IAuditWriteRepository _auditWriteRepository;
    private readonly IAttendancesWriteRepository _attendancesWriteRepository;
    private readonly INotificationsWriteRepository _notificationsWriteRepository;
    private readonly IMembersReadRepository _membersReadRepository;
    private readonly IAanDataContext _aanDataContext;

    public PutCalendarEventCommandHandler(ICalendarEventsWriteRepository calendarEventWriteRepository, IAuditWriteRepository auditWriteRepository, IAanDataContext aanDataContext, IAttendancesWriteRepository attendancesWriteRepository, INotificationsWriteRepository notificationsWriteRepository, IMembersReadRepository membersReadRepository)
    {
        _calendarEventWriteRepository = calendarEventWriteRepository;
        _auditWriteRepository = auditWriteRepository;
        _aanDataContext = aanDataContext;
        _attendancesWriteRepository = attendancesWriteRepository;
        _notificationsWriteRepository = notificationsWriteRepository;
        _membersReadRepository = membersReadRepository;
    }

    public async Task<ValidatedResponse<SuccessCommandResult>> Handle(PutCalendarEventCommand command, CancellationToken cancellationToken)
    {
        var existingEvent = await _calendarEventWriteRepository.GetCalendarEvent(command.CalendarEventId);

        var audit = new Audit()
        {
            Action = "Put",
            Before = JsonSerializer.Serialize(existingEvent),
            ActionedBy = command.AdminMemberId,
            AuditTime = DateTime.UtcNow,
            Resource = nameof(CalendarEvent),
        };

        UpdateCalendarEventToNewValues(command, existingEvent);

        audit.After = JsonSerializer.Serialize(existingEvent);
        _auditWriteRepository.Create(audit);

        if (command.SendUpdateEventNotification)
        {
            var existingAttendances =
                await _attendancesWriteRepository.GetAttendancesByEventId(command.CalendarEventId, cancellationToken);

            if (existingAttendances.Any())
            {
                var members = await _membersReadRepository.GetMembers(
                    existingAttendances.Select(x => x.MemberId).ToList(),
                    cancellationToken);

                foreach (var attendance in existingAttendances)
                {
                    var member = members.First(x => x.Id == attendance.MemberId);
                    var templateName = Constants.NotificationTemplateNames.AANAdminEventUpdate;
                    var tokens = await GetTokens(existingEvent, member);
                    var notification = NotificationHelper.CreateNotification(Guid.NewGuid(), attendance.MemberId,
                        templateName, tokens, command.AdminMemberId, true, command.CalendarEventId.ToString());
                    _notificationsWriteRepository.Create(notification);
                }
            }
        }

        await _aanDataContext.SaveChangesAsync(cancellationToken);
        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
    }

    private static void UpdateCalendarEventToNewValues(PutCalendarEventCommand command, CalendarEvent existingEvent)
    {
        existingEvent.CalendarId = command.CalendarId.GetValueOrDefault();
        existingEvent.EventFormat = command.EventFormat!.Value.ToString();
        existingEvent.StartDate = command.StartDate.GetValueOrDefault();
        existingEvent.EndDate = command.EndDate.GetValueOrDefault();
        existingEvent.Title = command.Title!;
        existingEvent.Description = command.Description!;
        existingEvent.Summary = command.Summary!;
        existingEvent.RegionId = command.RegionId;
        existingEvent.Location = command.Location;
        existingEvent.Postcode = command.Postcode!;
        existingEvent.Latitude = command.Latitude;
        existingEvent.Longitude = command.Longitude;
        existingEvent.EventLink = command.EventLink;
        existingEvent.ContactName = command.ContactName!;
        existingEvent.ContactEmail = command.ContactEmail!;
        existingEvent.PlannedAttendees = command.PlannedAttendees.GetValueOrDefault();
        existingEvent.Urn = command.Urn;
        existingEvent.LastUpdatedDate = DateTime.UtcNow;
    }

    private static Task<string> GetTokens(CalendarEvent calendarEvent, Member member)
    {
        var date = calendarEvent!.StartDate.ToString("dd/MM/yyyy");
        var time = calendarEvent!.StartDate.ToString("HH:mm");
        var fullname = member.FullName;
        var eventName = calendarEvent.Title;

        var tokens = new Dictionary<string, string>
        {
            { "contact", fullname },
            { "eventname", eventName },
            { "date", date },
            { "time", time }
        };

        return Task.FromResult(JsonSerializer.Serialize(tokens));
    }
}