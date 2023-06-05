using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries;

public class GetCalendarEventsQuery : IRequest<ValidatedResponse<GetCalendarEventsQueryResult>>, IRequestedByMemberId
{
    public Guid RequestedByMemberId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public List<EventFormat> EventFormat { get; set; }
    public int? Page { get; }

    public List<int> CalendarId { get; set; }

    public GetCalendarEventsQuery(Guid requestedByMemberId, DateTime? fromDate, DateTime? toDate, List<EventFormat> eventFormat, List<int> calendarId, int? page)
    {
        RequestedByMemberId = requestedByMemberId;
        FromDate = fromDate;
        ToDate = toDate;
        Page = page;
        EventFormat = eventFormat;
        CalendarId = calendarId;
    }
}