namespace SFA.DAS.AANHub.Domain.Models;

public class CalendarEventSummaryProcessed : CalendarEventSummaryBase
{
    public static implicit operator CalendarEventSummaryProcessed(CalendarEventSummary summary) => new()
    {
        CalendarEventId = summary.CalendarEventId,
        CalendarName = summary.CalendarName,
        EventFormat = summary.EventFormat,
        Start = summary.Start,
        End = summary.End,
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