using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

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
        var pageSize = Domain.Common.Constants.CalendarEvents.PageSize;
        var page = 1;
        var response =
            await _calendarEventsReadRepository.GetCalendarEvents(request.RequestedByMemberId, cancellationToken);

        var result = new GetCalendarEventsQueryResult
        {
            Page = page,
            PageSize = 0,
            TotalCount = response.Count,
            TotalPages = 0,
            CalendarEvents = response.ToList()
        };

        return new ValidatedResponse<GetCalendarEventsQueryResult>(result);
    }
}