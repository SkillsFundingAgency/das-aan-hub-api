using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;

public class CalendarEventSummaryModel
{
    public Guid CalendarEventId { get; set; }
    public string? CalendarName { get; set; } = null!;
    public string EventFormat { get; set; } = null!;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Summary { get; set; }
    public string? Location { get; set; }
    public string? Postcode { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public double? Distance { get; set; }
    public bool IsAttending { get; set; }

    public static implicit operator CalendarEventSummaryModel(CalendarEventSummary summary) => new()
    {
        CalendarEventId = summary.CalendarEventId,
        CalendarName = summary.CalendarName,
        EventFormat = summary.EventFormat,
        Start = summary.Start,
        End = summary.End,
        Title = summary.Title,
        Description = summary.Description,
        Summary = summary.Summary,
        Location = summary.Location,
        Postcode = summary.Postcode,
        Longitude = summary.Longitude,
        Latitude = summary.Latitude,
        Distance = summary.Distance,
        IsAttending = summary.IsAttending
    };
}