﻿using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.AANHub.Domain.Common;
using Constants = SFA.DAS.AANHub.Api.Common.Constants;

namespace SFA.DAS.AANHub.Api.Models;

public class GetCalendarEventsModel
{
    [FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)]
    public Guid RequestedByMemberId { get; set; }

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
    public int Page { get; set; } = 1;

    [FromQuery] public int PageSize { get; set; } = Domain.Common.Constants.CalendarEvents.PageSize;

    public static implicit operator GetCalendarEventsQuery(GetCalendarEventsModel model) => new()
    {
        RequestedByMemberId = model.RequestedByMemberId,
        FromDate = model.FromDate,
        ToDate = model.ToDate,
        EventFormats = model.EventFormat,
        RegionIds = model.RegionId,
        CalendarIds = model.CalendarId,
        Page = model.Page,
        PageSize = model.PageSize
    };


}
