﻿namespace SFA.DAS.AANHub.Domain.Models;

public class CalendarEventSummaryBase
{
    public Guid CalendarEventId { get; set; }
    public string? CalendarName { get; set; } = null!;
    public string EventFormat { get; set; } = null!;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Description { get; set; } = null!;
    public string? Summary { get; set; }
    public string? Location { get; set; }
    public string? Postcode { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public double? Distance { get; set; }
    public bool IsAttending { get; set; }
}