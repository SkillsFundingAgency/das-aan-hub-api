
using MediatR;
using SFA.DAS.AAN.Application.Commands.CalendarEventCommands;
using SFA.DAS.AAN.Domain.Enums;


namespace SFA.DAS.AAN.Application.Commands.CreateCalendarEvent
{
    public class CreateCalendarEventCommand : CalendarEventCommandBase, IRequest<CreateCalendarEventResponse>
    {
    }
}
