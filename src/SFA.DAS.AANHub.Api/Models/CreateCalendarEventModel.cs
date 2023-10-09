using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Api.Models;

public class CreateCalendarEventModel
{
    public int? CalendarId { get; set; }
    public EventFormat? EventFormat { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Summary { get; set; }
    public int? RegionId { get; set; }
    public string? Location { get; set; }
    public string? Postcode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? EventLink { get; set; }
    public string? ContactName { get; set; }
    public string? ContactEmail { get; set; }
    public int? PlannedAttendees { get; set; }
    public long? Urn { get; set; }

    public static implicit operator CreateCalendarEventCommand(CreateCalendarEventModel source) =>
        new()
        {
            CalendarId = source.CalendarId,
            EventFormat = source.EventFormat,
            StartDate = source.StartDate,
            EndDate = source.EndDate,
            Title = source.Title,
            Description = source.Description!,
            Summary = source.Summary,
            RegionId = source.RegionId == 0 ? null : source.RegionId,
            Location = source.Location,
            Postcode = source.Postcode,
            Latitude = source.Latitude,
            Longitude = source.Longitude,
            EventLink = source.EventLink,
            ContactName = source.ContactName,
            ContactEmail = source.ContactEmail,
            PlannedAttendees = source.PlannedAttendees,
            Urn = source.Urn
        };
}
