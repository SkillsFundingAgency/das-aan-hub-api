using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Domain.Models;
public class GetCalendarEventsOptions
{
    public Guid MemberId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public List<EventFormat> EventFormats { get; set; }
    public List<int> CalendarIds { get; set; }
    public List<int> RegionIds { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}