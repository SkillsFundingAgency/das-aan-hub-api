using FluentValidation;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries;
public class GetCalendarEventsQueryValidator : AbstractValidator<GetCalendarEventsQuery>
{
    public GetCalendarEventsQueryValidator()
    {
        RuleFor(a => a.RequestedByMemberId)
            .NotEmpty();
    }
}
