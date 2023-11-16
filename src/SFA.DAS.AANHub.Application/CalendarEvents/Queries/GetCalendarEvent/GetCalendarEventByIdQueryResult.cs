using SFA.DAS.AANHub.Application.Models;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;

public class GetCalendarEventByIdQueryResult
{
    public Guid CalendarEventId { get; set; }
    public string? CalendarName { get; set; } = null!;
    public int CalendarId { get; set; }
    public string EventFormat { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Location { get; set; }
    public string? Postcode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? EventLink { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public List<AttendeeModel> Attendees { get; set; } = null!;
    public List<EventGuestModel> EventGuests { get; set; } = null!;
    public int PlannedAttendees { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? RegionId { get; set; }
    public long? Urn { get; set; }

    public static implicit operator GetCalendarEventByIdQueryResult(CalendarEvent source)
    {
        if (source is not null) return new()
        {
            CalendarEventId = source.Id,
            CalendarName = source.Calendar.CalendarName,
            CalendarId = source.CalendarId,
            EventFormat = source.EventFormat,
            StartDate = source.StartDate,
            EndDate = source.EndDate,
            Title = source.Title,
            Description = source.Description!,
            Summary = source.Summary,
            Location = source.Location,
            Postcode = source.Postcode,
            Latitude = source.Latitude,
            Longitude = source.Longitude,
            EventLink = source.EventLink,
            ContactName = source.ContactName,
            ContactEmail = source.ContactEmail,
            IsActive = source.IsActive,
            LastUpdatedDate = source.LastUpdatedDate,
            Attendees = source.Attendees.Select(a => (AttendeeModel)a).ToList(),
            EventGuests = source.EventGuests.Select(e => (EventGuestModel)e).ToList(),
            RegionId = source.RegionId,
            Urn = source.Urn,
            PlannedAttendees = source.PlannedAttendees,
            CreatedDate = source.CreatedDate
        };

        return null!;
    }
}
