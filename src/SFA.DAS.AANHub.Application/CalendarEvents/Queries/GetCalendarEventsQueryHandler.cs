using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries;

public class GetCalendarEventsQueryHandler : IRequestHandler<GetCalendarEventsQuery, ValidatedResponse<GetCalendarEventsQueryResult>>
{
    private readonly ICalendarEventsReadRepository _calendarEventsReadRepository;

    public GetCalendarEventsQueryHandler(ICalendarEventsReadRepository calendarEventsReadRepository)
    {
        _calendarEventsReadRepository = calendarEventsReadRepository;
    }

    public async Task<ValidatedResponse<GetCalendarEventsQueryResult>> Handle(GetCalendarEventsQuery request, CancellationToken cancellationToken)
    {
        var pageSize = 0;
        var page = 1;
        var startDate = request.StartDate == null || request.StartDate.GetValueOrDefault() < DateTime.Today ? DateTime.Today : request.StartDate.GetValueOrDefault();

        var endDate = request.EndDate ?? DateTime.Today.AddYears(1);

        if (startDate > endDate)
        {
            return new ValidatedResponse<GetCalendarEventsQueryResult>(
                new GetCalendarEventsQueryResult
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = 0,
                    CalendarEvents = new List<CalendarEventSummary>()
                });
        }

        var response =
            await _calendarEventsReadRepository.GetCalendarEvents(request.RequestedByMemberId, startDate, endDate, cancellationToken);

        var result = new GetCalendarEventsQueryResult
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = response.Count,
            TotalPages = 0,
            CalendarEvents = response.ToList()
        };

        return new ValidatedResponse<GetCalendarEventsQueryResult>(result);
    }
}