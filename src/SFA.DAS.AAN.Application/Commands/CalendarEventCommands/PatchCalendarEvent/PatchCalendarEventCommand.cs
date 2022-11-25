
using MediatR;
using SFA.DAS.AAN.Application.Commands.CalendarEventCommands;


namespace SFA.DAS.AAN.Application.Commands.PatchCalendarEvent
{
    public class PatchCalendarEventCommand : CalendarEventCommandBase, IRequest<PatchCalendarEventResponse>
    {
        public Guid calendareventid { get; set; }
    }
}
