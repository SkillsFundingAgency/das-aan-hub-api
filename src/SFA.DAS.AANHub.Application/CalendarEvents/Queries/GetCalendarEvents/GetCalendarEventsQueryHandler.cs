using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;


namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;

public class GetCalendarEventsQueryHandler : IRequestHandler<GetCalendarEventsQuery, ValidatedResponse<GetCalendarEventsQueryResult>>
{
    private readonly ICalendarEventsReadRepository _calendarEventsReadRepository;

    public GetCalendarEventsQueryHandler(ICalendarEventsReadRepository calendarEventsReadRepository)
    {
        _calendarEventsReadRepository = calendarEventsReadRepository;
    }

    public async Task<ValidatedResponse<GetCalendarEventsQueryResult>> Handle(GetCalendarEventsQuery request,
        CancellationToken cancellationToken)
    {
        var pageSize = request.PageSize;
        var page = request.Page;
        var fromDate = request.FromDate == null || request.FromDate.GetValueOrDefault() < DateTime.Today
            ? DateTime.Today
            : request.FromDate.GetValueOrDefault();

        var toDate = request.ToDate ?? DateTime.Today.AddYears(1);

        if (fromDate > toDate)
        {
            return new ValidatedResponse<GetCalendarEventsQueryResult>(
                new GetCalendarEventsQueryResult
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = 0,
                    CalendarEvents = new List<CalendarEventSummaryModel>()
                });
        }

        var options = new GetCalendarEventsOptions(request.RequestedByMemberId, fromDate, toDate, request.EventFormats,
            request.CalendarIds, request.RegionIds, page, pageSize);

        var response =
            await _calendarEventsReadRepository.GetCalendarEvents(options, cancellationToken);

        var totalCount = 0;

        if (response.Any())
        {
            totalCount = response.First().TotalCount;
        }

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var responseProcessed = response.Select(summary => (CalendarEventSummaryModel)summary).ToList();

        var result = new GetCalendarEventsQueryResult
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            CalendarEvents = responseProcessed
        };

        return new ValidatedResponse<GetCalendarEventsQueryResult>(result);
    }
}