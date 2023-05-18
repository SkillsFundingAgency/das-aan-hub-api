namespace SFA.DAS.AANHub.Domain.Models;

public class CalendarEventModel
{
    public Guid CalendarEventId { get; set; }
    public string CalendarName { get; set; } = string.Empty;
    public string EventFormat { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public string Description { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Postcode { get; set; } = string.Empty;
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public double? Distance { get; set; }
    public bool IsAttending { get; set; }
}