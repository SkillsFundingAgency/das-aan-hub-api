
using MediatR;
using SFA.DAS.AANHub.Application.Commands.CalendarEventCommands;
using SFA.DAS.AANHub.Domain.Enums;


namespace SFA.DAS.AANHub.Application.Commands.CreateCalendarEvent
{
    public class CreateCalendarEventCommand : CalendarEventCommandBase, IRequest<CreateCalendarEventResponse>
    {
    }
}
