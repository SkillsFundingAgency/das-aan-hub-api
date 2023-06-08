using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;

public class GetCalendarEventsQueryValidator : AbstractValidator<GetCalendarEventsQuery>
{
    public GetCalendarEventsQueryValidator(IMembersReadRepository membersReadRepository)
    {
        Include(new RequestedByMemberIdValidator(membersReadRepository));
    }
}
