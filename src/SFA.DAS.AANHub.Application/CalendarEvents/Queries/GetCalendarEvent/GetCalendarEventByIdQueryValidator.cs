using FluentValidation;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;

public class GetCalendarEventByIdQueryValidator : AbstractValidator<GetCalendarEventByIdQuery>
{
    public GetCalendarEventByIdQueryValidator()
    {
        RuleFor(a => a.CalendarEventId)
            .NotEmpty();
    }
}