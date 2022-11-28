
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SFA.DAS.AAN.Domain.Entities;
using SFA.DAS.AAN.Domain.Interfaces;


namespace SFA.DAS.AAN.Application.Commands.CreateCalendarEvent
{
    public class CreateCalendarEventCommandHandler : IRequestHandler<CreateCalendarEventCommand, CreateCalendarEventResponse>
    {
        private readonly ICalendarEventsContext _calendarEventsContext;

        public CreateCalendarEventCommandHandler(ICalendarEventsContext calendarEventsContext)
        {
            _calendarEventsContext = calendarEventsContext;
        }

        public async Task<CreateCalendarEventResponse> Handle(CreateCalendarEventCommand command, CancellationToken cancellationToken)
        {
            EntityEntry<CalendarEvent> calendarEvent = await _calendarEventsContext.Entities.AddAsync(
                new CalendarEvent()
                {
                    Id = Guid.NewGuid(),
                    CalendarId = command.calendarid,
                    Start = command.start,
                    End = command.end,
                    Description = command.description,
                    RegionId = command.regionid,
                    Location = command.location,
                    Postcode = command.postcode,
                    EventLink = command.eventlink,
                    Contact = command.contact,
                    ContactEmail = command.email,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    CreatedByUserId = command.userid,
                    UpdatedByUserId = command.userid,
                    IsActive = true
                }, cancellationToken
            );
            await _calendarEventsContext.SaveChangesAsync(cancellationToken);

            CalendarEvent result = calendarEvent.Entity;
            return new CreateCalendarEventResponse()
            {
                calendareventid = result.Id,
                created = result.Created
            };
        }
    }
}
