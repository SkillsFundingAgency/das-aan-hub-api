using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Domain.Models;
public class GetCalendarEventsOptions
{
    public Guid MemberId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public List<EventFormat> EventFormats { get; set; }
    public List<int> CalendarIds { get; set; }
    public int? Page { get; }

    public GetCalendarEventsOptions(Guid requestedByMemberId, DateTime? fromDate, DateTime? toDate, List<EventFormat> eventFormats, List<int> calendarIds, int? page)
    {
        MemberId = requestedByMemberId;
        FromDate = fromDate;
        ToDate = toDate;
        EventFormats = eventFormats;
        CalendarIds = calendarIds;
        Page = page;
    }
}
