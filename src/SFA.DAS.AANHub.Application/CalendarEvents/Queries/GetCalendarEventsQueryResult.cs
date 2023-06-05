using SFA.DAS.AANHub.Domain.Models;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries;
public class GetCalendarEventsQueryResult
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public List<CalendarEventSummaryProcessed> CalendarEvents { get; set; } = new List<CalendarEventSummaryProcessed>();
}