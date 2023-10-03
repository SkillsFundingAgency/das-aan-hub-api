using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;

public class CreateCalendarEventCommand
{
    public int CalendarId { get; set; }
    public EventFormat? EventFormat { get; set; }
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
    public int PlannedAttendees { get; set; }
    public long? Urn { get; set; }
}
