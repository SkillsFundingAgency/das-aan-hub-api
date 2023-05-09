using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;

public class GetCalendarEventByIdQueryValidator : AbstractValidator<GetCalendarEventByIdQuery>
{
    public const string CalendarEventIdMissingMessage = "CalendarEventId must have a value";

    public GetCalendarEventByIdQueryValidator(IMembersReadRepository membersReadRepository)
    {
        Include(new RequestedByMemberIdValidator(membersReadRepository));

        RuleFor(a => a.CalendarEventId)
            .NotEmpty()
            .WithMessage(CalendarEventIdMissingMessage);
    }
}