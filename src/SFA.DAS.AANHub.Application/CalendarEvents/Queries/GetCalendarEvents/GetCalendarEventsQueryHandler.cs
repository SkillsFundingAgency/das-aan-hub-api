using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using System.Text.RegularExpressions;


namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;

public class GetCalendarEventsQueryHandler : IRequestHandler<GetCalendarEventsQuery, ValidatedResponse<GetCalendarEventsQueryResult>>
{
    private readonly ICalendarEventsReadRepository _calendarEventsReadRepository;

    public GetCalendarEventsQueryHandler(ICalendarEventsReadRepository calendarEventsReadRepository)
    {
        _calendarEventsReadRepository = calendarEventsReadRepository;
    }

    public async Task<ValidatedResponse<GetCalendarEventsQueryResult>> Handle(GetCalendarEventsQuery query,
        CancellationToken cancellationToken)
    {
        var pageSize = query.PageSize;
        var page = query.Page;

        var fromDate = query.FromDate == null || query.FromDate.GetValueOrDefault() < DateTime.Today
            ? DateTime.Today
            : query.FromDate.GetValueOrDefault();

        var toDate = query.ToDate ?? DateTime.Today.AddYears(1);

        if (fromDate > toDate)
        {
            return new ValidatedResponse<GetCalendarEventsQueryResult>(
                new GetCalendarEventsQueryResult
                {
                    Page = 0,
                    PageSize = pageSize,
                    TotalCount = 0,
                    CalendarEvents = new List<CalendarEventSummaryModel>()
                });
        }

        var options = new GetCalendarEventsOptions
        {
            MemberId = query.RequestedByMemberId,
            Keyword = ProcessedKeyword(query.Keyword),
            FromDate = fromDate,
            ToDate = toDate,
            EventFormats = query.EventFormats,
            CalendarIds = query.CalendarIds,
            RegionIds = query.RegionIds,
            Page = page,
            PageSize = pageSize
        };

        var response =
            await _calendarEventsReadRepository.GetCalendarEvents(options, cancellationToken);

        var totalCount = 0;

        if (response.Any())
        {
            totalCount = response[0].TotalCount;
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

    private static string? ProcessedKeyword(string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword)) return null;
        var rgx = new Regex("[^a-zA-Z0-9 ]", RegexOptions.None, TimeSpan.FromMilliseconds(100));
        return rgx.Replace(keyword, " ").Trim();

    }
}