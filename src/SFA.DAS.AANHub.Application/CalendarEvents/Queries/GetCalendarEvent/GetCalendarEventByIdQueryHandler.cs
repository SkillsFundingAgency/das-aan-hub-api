using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;

public class GetCalendarEventByIdQueryHandler : IRequestHandler<GetCalendarEventByIdQuery, ValidatedResponse<GetCalendarEventByIdQueryResult>>
{
    private readonly ICalendarEventsReadRepository _calendarEventsReadRepository;
    private readonly ICalendarsReadRepository _calendarReadRepository;

    public GetCalendarEventByIdQueryHandler(ICalendarEventsReadRepository calendarEventsReadRepository, ICalendarsReadRepository calendarsReadRepository)
    {
        _calendarEventsReadRepository = calendarEventsReadRepository;
        _calendarReadRepository = calendarsReadRepository;
    }

    public async Task<ValidatedResponse<GetCalendarEventByIdQueryResult>> Handle(GetCalendarEventByIdQuery request, CancellationToken cancellationToken)
    {
        var calendarEvent = await _calendarEventsReadRepository.GetCalendarEvent(request.CalendarEventId);

        GetCalendarEventByIdQueryResult result = calendarEvent!;

        if (calendarEvent != null)
        {
            result.CalendarName = await _calendarReadRepository.GetCalendarName(calendarEvent!.CalendarId);
        }

        return new ValidatedResponse<GetCalendarEventByIdQueryResult>(result!);
    }
}
