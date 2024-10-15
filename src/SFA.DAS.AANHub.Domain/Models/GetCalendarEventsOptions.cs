using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Domain.Models;
public class GetCalendarEventsOptions
{
    public Guid MemberId { get; set; }
    public string? Keyword { get; set; }

    public int KeywordCount => string.IsNullOrWhiteSpace(Keyword) ? 0 : Keyword.Split(" ").Length;

    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public List<EventFormat> EventFormats { get; set; } = new List<EventFormat>();
    public List<int> CalendarIds { get; set; } = new List<int>();
    public List<int> RegionIds { get; set; } = new List<int>();
    public bool? IsActive { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool ShowUserEventsOnly { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? Radius { get; set; }
    public string? OrderEventsBy { get; set; }
}