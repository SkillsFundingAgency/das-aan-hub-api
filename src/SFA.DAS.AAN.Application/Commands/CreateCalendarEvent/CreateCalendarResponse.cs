
using SFA.DAS.AAN.Domain.Entities;


namespace SFA.DAS.AAN.Application.Commands.CreateCalendarEvent
{
    public class CreateCalendarEventResponse
    {
        public Guid calendareventid { get; set; }
        public DateTime created { get; set; }
        public string? warnings { get; set; }
    }
}
