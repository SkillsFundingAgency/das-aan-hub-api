using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.DeleteCalendarEvent;

public class DeleteCalendarEventCommand : IRequest<ValidatedResponse<SuccessCommandResult>>, IRequestedByMemberId
{
    public Guid CalendarEventId { get; set; }
    public Guid RequestedByMemberId { get; set; }

    public DeleteCalendarEventCommand(Guid calendarEventId, Guid requestedByMemberId)
    {
        CalendarEventId = calendarEventId;
        RequestedByMemberId = requestedByMemberId;
    }
}
