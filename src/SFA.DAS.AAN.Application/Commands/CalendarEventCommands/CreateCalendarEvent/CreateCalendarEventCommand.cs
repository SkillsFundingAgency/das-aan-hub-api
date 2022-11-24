
using MediatR;
using SFA.DAS.AAN.Application.Commands.CalendarEventCommands;
using SFA.DAS.AAN.Domain.Enums;


namespace SFA.DAS.AAN.Application.Commands.CreateCalendarEvent
{
    public class CreateCalendarEventCommand : CalendarEventCommandBase, IRequest<CreateCalendarEventResponse>
    {
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
