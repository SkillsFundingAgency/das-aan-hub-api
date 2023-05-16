using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;

public class GetCalendarEventByIdQueryHandler : IRequestHandler<GetCalendarEventByIdQuery, ValidatedResponse<GetCalendarEventByIdQueryResult>>
{
    private readonly ICalendarEventsReadRepository _calendarEventsReadRepository;

    public GetCalendarEventByIdQueryHandler(ICalendarEventsReadRepository calendarEventsReadRepository) => _calendarEventsReadRepository = calendarEventsReadRepository;

    public async Task<ValidatedResponse<GetCalendarEventByIdQueryResult>> Handle(GetCalendarEventByIdQuery request, CancellationToken cancellationToken)
    {
        var calendarEvent = await _calendarEventsReadRepository.GetCalendarEvent(request.CalendarEventId);

        return new ValidatedResponse<GetCalendarEventByIdQueryResult>(calendarEvent!);
    }
}
