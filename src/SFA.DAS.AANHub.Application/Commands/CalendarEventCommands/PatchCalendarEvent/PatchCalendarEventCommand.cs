
using MediatR;
using SFA.DAS.AANHub.Application.Commands.CalendarEventCommands;


namespace SFA.DAS.AANHub.Application.Commands.PatchCalendarEvent
{
    public class PatchCalendarEventCommand : CalendarEventCommandBase, IRequest<PatchCalendarEventResponse>
    {
        public Guid calendareventid { get; set; }
    }
}
