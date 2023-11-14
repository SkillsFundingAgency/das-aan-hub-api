using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
public class PutCalendarEventCommand : CalendarEventCommandBase.CalendarEventCommandBase, IRequest<ValidatedResponse<SuccessCommandResult>>
{
    public Guid CalendarEventId { get; set; }
    public bool SendUpdateEventNotification { get; set; }
}
