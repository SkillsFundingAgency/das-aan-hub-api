using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries;

public class GetCalendarEventsQuery : IRequest<ValidatedResponse<GetCalendarEventsQueryResult>>, IRequestedByMemberId
{
    public Guid RequestedByMemberId { get; set; }
    public int? Page { get; }

    public GetCalendarEventsQuery(Guid requestedByMemberId, int? page)
    {
        RequestedByMemberId = requestedByMemberId;
        Page = page;
    }
}
