using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;

public class GetCalendarEventsQuery : IRequest<GetCalendarEventsQueryResult>, IRequestedByMemberId
{
    public Guid RequestedByMemberId { get; set; }
    public string? Keyword { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public List<EventFormat> EventFormats { get; set; } = new List<EventFormat>();
    public List<int> CalendarIds { get; set; } = new List<int>();
    public List<int> RegionIds { get; set; } = new List<int>();

    public bool? IsActive { get; set; }
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = Domain.Common.Constants.CalendarEvents.PageSize;
    public bool ShowUserEventsOnly { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? Radius { get; set; }
    public string OrderBy { get; set; } = string.Empty;
}