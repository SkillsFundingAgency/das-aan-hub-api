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
        var fromDate = request.FromDate == null || request.FromDate.GetValueOrDefault() < DateTime.Today ? DateTime.Today : request.FromDate.GetValueOrDefault();

        var toDate = request.ToDate ?? DateTime.Today.AddYears(1);

        if (fromDate > toDate)
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
            await _calendarEventsReadRepository.GetCalendarEvents(request.RequestedByMemberId, fromDate, toDate, request.EventFormats, cancellationToken);

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