
using MediatR;
using SFA.DAS.AAN.Domain.Entities;
using SFA.DAS.AAN.Domain.Interfaces;


namespace SFA.DAS.AAN.Application.Commands.PatchCalendarEvent
{
    public class PatchCalendarEventCommandHandler : IRequestHandler<PatchCalendarEventCommand, PatchCalendarEventResponse>
    {
        private readonly ICalendarEventsContext _calendarEventsContext;

        public PatchCalendarEventCommandHandler(ICalendarEventsContext calendarEventsContext)
        {
            _calendarEventsContext = calendarEventsContext;
        }

        public async Task<PatchCalendarEventResponse> Handle(PatchCalendarEventCommand command, CancellationToken cancellationToken)
        {
            List<string> warnings = new List<string>();
            CalendarEvent? calendarEvent =
                await _calendarEventsContext.Entities.FindAsync(new object?[] { command.calendareventid }, cancellationToken: cancellationToken);

            if (calendarEvent == null)
                warnings.Add($"Calendar event with id {command.calendareventid} not found");

            else
            {
                calendarEvent.Start = command.start;
                calendarEvent.End = command.end;
                calendarEvent.Description = command.description;
                calendarEvent.RegionId = command.regionid;
                calendarEvent.Location = command.location;
                calendarEvent.Postcode = command.postcode;
                calendarEvent.EventLink = command.eventlink;
                calendarEvent.Contact = command.contact;
                calendarEvent.ContactEmail = command.email;
                calendarEvent.Updated = DateTime.Now;
                calendarEvent.UpdatedByUserId = command.userid;

                await _calendarEventsContext.SaveChangesAsync(cancellationToken);
            }

            return new PatchCalendarEventResponse()
            {
                warnings = warnings
            };
        }
    }
}
