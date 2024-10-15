using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.AANHub.Domain.Common;
using Constants = SFA.DAS.AANHub.Api.Common.Constants;

namespace SFA.DAS.AANHub.Api.Models;

public class GetCalendarEventsModel
{
    [FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)]
    public Guid RequestedByMemberId { get; set; }

    [FromQuery]
    public string Keyword { get; set; } = string.Empty;

    [FromQuery]
    public DateTime? FromDate { get; set; }

    [FromQuery]
    public DateTime? ToDate { get; set; }

    [FromQuery]
    public List<EventFormat> EventFormat { get; set; } = new List<EventFormat>();

    [FromQuery]
    public List<int> CalendarId { get; set; } = new List<int>();

    [FromQuery]
    public List<int> RegionId { get; set; } = new List<int>();

    [FromQuery]
    public bool? IsActive { get; set; }

    [FromQuery]
    public bool ShowUserEventsOnly { get; set; }

    [FromQuery]
    public int Page { get; set; } = 1;

    [FromQuery] public int PageSize { get; set; } = Domain.Common.Constants.CalendarEvents.PageSize;

    [FromQuery]
    public double? Latitude { get; set; }

    [FromQuery]
    public double? Longitude { get; set; }

    [FromQuery]
    public int? Radius { get; set; }

    [FromQuery]
    public string OrderEventsBy { get; set; }

    public static implicit operator GetCalendarEventsQuery(GetCalendarEventsModel model) => new()
    {
        RequestedByMemberId = model.RequestedByMemberId,
        Keyword = model.Keyword,
        FromDate = model.FromDate,
        ToDate = model.ToDate,
        EventFormats = model.EventFormat,
        RegionIds = model.RegionId,
        CalendarIds = model.CalendarId,
        IsActive = model.IsActive,
        Page = model.Page,
        PageSize = model.PageSize,
        ShowUserEventsOnly = model.ShowUserEventsOnly,
        Latitude = model.Latitude,
        Longitude = model.Longitude,
        Radius = model.Radius,
        OrderEventsBy = model.OrderEventsBy
    };
}
