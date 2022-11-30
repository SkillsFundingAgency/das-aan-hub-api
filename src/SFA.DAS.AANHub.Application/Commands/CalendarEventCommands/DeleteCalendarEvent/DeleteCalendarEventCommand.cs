
using MediatR;


namespace SFA.DAS.AANHub.Application.Commands.DeleteCalendarEvent
{
    public class DeleteCalendarEventCommand : IRequest<DeleteCalendarEventResponse>
    {
        public Guid calendareventid { get; set; }
        public Guid userid { get; set; }
        public string? reason { get; set; }
        public IEnumerable<string>? notifications { get; set; }
    }
}
