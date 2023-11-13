using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
public class PutCalendarEventCommand : IRequest<ValidatedResponse<SuccessCommandResult>>
{
    public Guid AdminMemberId { get; set; }
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
    public bool SendUpdateEventNotification { get; set; }

    public static implicit operator CalendarEvent(PutCalendarEventCommand source) =>
        new()
        {
            CalendarId = source.CalendarId!.Value,
            EventFormat = source.EventFormat!.Value.ToString(),
            StartDate = source.StartDate!.Value,
            EndDate = source.EndDate!.Value,
            Title = source.Title!,
            Description = source.Description!,
            Summary = source.Summary!,
            RegionId = source.RegionId,
            Location = source.Location,
            Postcode = source.Postcode,
            Latitude = source.Latitude,
            Longitude = source.Longitude,
            EventLink = source.EventLink,
            ContactName = source.ContactName!,
            ContactEmail = source.ContactEmail!,
            PlannedAttendees = source.PlannedAttendees!.Value,
            Urn = source.Urn
        };
}
