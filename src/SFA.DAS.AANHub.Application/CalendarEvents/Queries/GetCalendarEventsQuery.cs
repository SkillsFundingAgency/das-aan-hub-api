﻿using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries;

public class GetCalendarEventsQuery : IRequest<ValidatedResponse<GetCalendarEventsQueryResult>>, IRequestedByMemberId
{
    public Guid RequestedByMemberId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<EventFormat> EventFormats { get; set; }
    public int? Page { get; }

    public GetCalendarEventsQuery(Guid requestedByMemberId, DateTime? startDate, DateTime? endDate, List<EventFormat> eventFormats, int? page)
    {
        RequestedByMemberId = requestedByMemberId;
        StartDate = startDate;
        EndDate = endDate;
        Page = page;
        EventFormats = eventFormats;
    }
}
