using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Domain.Entities;

public class CalendarEvent
{
    public Guid Id { get; set; }
    public int CalendarId { get; set; }
    public string EventFormat { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Summary { get; set; }
    public int? RegionId { get; set; }
    public string? Location { get; set; }
    public string? Postcode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? EventLink { get; set; }
    public string ContactName { get; set; } = null!;
    public string ContactEmail { get; set; } = null!;
    public bool IsActive { get; set; }
    public int PlannedAttendees { get; set; }
    public long? Urn { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public Guid CreatedByMemberId { get; set; }
    [JsonIgnore]
    public virtual List<Attendance> Attendees { get; set; } = new();

    [JsonIgnore]
    public Calendar Calendar { get; set; } = null!;

    [JsonIgnore]
    public virtual List<EventGuest> EventGuests { get; set; } = new();
}
