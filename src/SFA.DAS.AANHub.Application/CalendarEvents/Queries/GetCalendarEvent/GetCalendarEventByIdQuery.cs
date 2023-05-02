using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent
{
    public class GetCalendarEventByIdQuery : IRequest<ValidatedResponse<GetCalendarEventByIdQueryResult>>
    {
        public GetCalendarEventByIdQuery(Guid calendarEventId) => CalendarEventId = calendarEventId;
        public Guid CalendarEventId { get; }
    }
}