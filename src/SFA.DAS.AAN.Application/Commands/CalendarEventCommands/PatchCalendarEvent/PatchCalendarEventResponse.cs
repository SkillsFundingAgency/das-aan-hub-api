
using SFA.DAS.AAN.Domain.Entities;


namespace SFA.DAS.AAN.Application.Commands.PatchCalendarEvent
{
    public class PatchCalendarEventResponse
    {
        public IEnumerable<string>? warnings { get; set; }
    }
}
