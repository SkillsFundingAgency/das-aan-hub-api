using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;

public class GetCalendarEventsQuery : IRequest<ValidatedResponse<GetCalendarEventsQueryResult>>, IRequestedByMemberId
{
    public Guid RequestedByMemberId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public List<EventFormat> EventFormats { get; set; }
    public List<int> CalendarIds { get; set; }
    public List<int> RegionIds { get; set; }
    public int Page { get; }
    public int PageSize { get; }
    public GetCalendarEventsQuery(Guid requestedByMemberId, DateTime? fromDate, DateTime? toDate, List<EventFormat> eventFormats, List<int> calendarIds, List<int> regionIds, int page, int pageSize)
    {
        RequestedByMemberId = requestedByMemberId;
        FromDate = fromDate;
        ToDate = toDate;
        Page = page;
        PageSize = pageSize;
        EventFormats = eventFormats;
        CalendarIds = calendarIds;
        RegionIds = regionIds;
    }
}