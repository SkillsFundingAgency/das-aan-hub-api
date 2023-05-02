using SFA.DAS.AANHub.Application.Models;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;

public class GetCalendarEventByIdQueryResult
{
    public Guid CalendarEventId { get; set; }
    public int CalendarId { get; set; }
    public string EventFormat { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public int? RegionId { get; set; }
    public string? Location { get; set; }
    public string? Postcode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? EventLink { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? CancelReason { get; set; } = null!;


    public List<AttendeeModel> Attendees { get; set; } = new();
    public List<EventGuestModel> EventGuests { get; set; } = new();

    public static implicit operator GetCalendarEventByIdQueryResult(CalendarEvent source)
    {
        if (source is not null) return new()
        {
            CalendarEventId = source.Id,
            CalendarId = source.CalendarId,
            EventFormat = source.EventFormat,
            StartDate = source.StartDate,
            EndDate = source.EndDate,
            Description = source.Description,
            Summary = source.Summary,
            RegionId = source.RegionId,
            Location = source.Location,
            Postcode = source.Postcode,
            Latitude = source.Latitude,
            Longitude = source.Longitude,
            EventLink = source.EventLink,
            ContactName = source.ContactName,
            ContactEmail = source.ContactEmail,
            IsActive = source.IsActive,
            CancelReason = source.CancelReason,

            Attendees = source.Attendees.Select(a => (AttendeeModel)a).ToList(),
            EventGuests = source.EventGuests.Select(e => (EventGuestModel)e).ToList(),
        };

        return null!;

    }
}
