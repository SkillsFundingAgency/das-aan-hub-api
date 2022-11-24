
using MediatR;
using SFA.DAS.AAN.Application.Commands.CalendarEventCommands;


namespace SFA.DAS.AAN.Application.Commands.PatchCalendarEvent
{
    public class PatchCalendarEventCommand : CalendarEventCommandBase, IRequest<PatchCalendarEventResponse>
    {
        public Guid calendareventid { get; set; }
        //public Guid userid { get; set; }
        //public long calendarid { get; set; }
        //public DateTime start { get; set; }
        //public DateTime end { get; set; }
        //public string description { get; set; }
        //public int regionid { get; set; }
        //public string? location { get; set; }
        //public string? postcode { get; set; }
        //public string? eventlink { get; set; }
        //public string contact { get; set; }
        //public string email { get; set; }
    }
}
