﻿namespace SFA.DAS.AANHub.Domain.Entities;

public class CalendarEvent
{
    public Guid Id { get; set; }
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


    public List<Attendance> Attendees { get; set; } = new();
    public List<EventGuest> EventGuests { get; set; } = new();
}