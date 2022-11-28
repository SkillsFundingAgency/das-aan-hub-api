
using MediatR;
using SFA.DAS.AAN.Domain.Entities;
using SFA.DAS.AAN.Domain.Interfaces;


namespace SFA.DAS.AAN.Application.Commands.DeleteCalendarEvent
{
    public class DeleteCalendarEventCommandHandler : IRequestHandler<DeleteCalendarEventCommand, DeleteCalendarEventResponse>
    {
        private readonly ICalendarEventsContext _calendarEventsContext;

        public DeleteCalendarEventCommandHandler(ICalendarEventsContext calendarEventsContext)
        {
            _calendarEventsContext = calendarEventsContext;
        }

        public async Task<DeleteCalendarEventResponse> Handle(DeleteCalendarEventCommand command, CancellationToken cancellationToken)
        {
            List<string> warnings = new List<string>();
            CalendarEvent? calendarEvent =
                await _calendarEventsContext.Entities.FindAsync(new object?[] { command.calendareventid }, cancellationToken: cancellationToken);

            if (calendarEvent == null)
                warnings.Add($"Calendar event with id {command.calendareventid} not found");

            else
            {
                _calendarEventsContext.Entities.Remove(calendarEvent);
                await _calendarEventsContext.SaveChangesAsync(cancellationToken);
            }

            return new DeleteCalendarEventResponse();
        }
    }
}
