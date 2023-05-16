using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent
{
    public class GetCalendarEventByIdQuery : IRequest<ValidatedResponse<GetCalendarEventByIdQueryResult>>, IRequestedByMemberId
    {
        public Guid CalendarEventId { get; }
        public Guid RequestedByMemberId { get; set; }

        public GetCalendarEventByIdQuery(Guid calendarEventId, Guid requestedByMemberId)
        {
            CalendarEventId = calendarEventId;
            RequestedByMemberId = requestedByMemberId;
        }
    }
}