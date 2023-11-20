using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;

public class CreateCalendarEventCommand : CalendarEventCommandBase.CalendarEventCommandBase, IRequest<ValidatedResponse<CreateCalendarEventCommandResult>>
{
    public static implicit operator CalendarEvent(CreateCalendarEventCommand source) =>
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