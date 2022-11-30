
namespace SFA.DAS.AANHub.Application.Commands.CreateCalendarEvent
{
    public class CreateCalendarEventResponse
    {
        public Guid calendareventid { get; set; }
        public DateTime created { get; set; }
        public IEnumerable<string>? warnings { get; set; }
    }
}
