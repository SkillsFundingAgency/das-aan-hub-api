using SFA.DAS.AANHub.Application.Models;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;

public class GetCalendarEventByIdQueryResult
{
    public CalendarEvent CalendarEvent { get; set; } = null!;
    public List<AttendeeModel> Attendees { get; set; } = null!;
    public List<EventGuestModel> EventGuests { get; set; } = null!;

    public static implicit operator GetCalendarEventByIdQueryResult(CalendarEvent source)
    {
        if (source is not null) return new()
        {
            CalendarEvent = source,
            Attendees = source.Attendees.Select(a => (AttendeeModel)a).ToList(),
            EventGuests = source.EventGuests.Select(e => (EventGuestModel)e).ToList(),
        };

        return null!;
    }
}
