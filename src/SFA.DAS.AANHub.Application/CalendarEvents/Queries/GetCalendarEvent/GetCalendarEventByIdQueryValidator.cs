using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;

public class GetCalendarEventByIdQueryValidator : AbstractValidator<GetCalendarEventByIdQuery>
{
    private readonly IMembersReadRepository _membersReadRepository;

    public GetCalendarEventByIdQueryValidator(IMembersReadRepository membersReadRepository)
    {
        _membersReadRepository = membersReadRepository;

        Include(new RequestedByMemberIdValidator(_membersReadRepository));

        RuleFor(a => a.CalendarEventId)
            .NotEmpty();
    }
}