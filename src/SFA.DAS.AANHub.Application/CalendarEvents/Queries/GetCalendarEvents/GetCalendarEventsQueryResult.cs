namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryResult
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public List<CalendarEventSummaryModel> CalendarEvents { get; set; } = new List<CalendarEventSummaryModel>();
}