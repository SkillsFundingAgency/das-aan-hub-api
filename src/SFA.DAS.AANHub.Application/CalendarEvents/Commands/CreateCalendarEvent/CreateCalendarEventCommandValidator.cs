using FluentValidation;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;

public class CreateCalendarEventCommandValidator : AbstractValidator<CreateCalendarEventCommand>
{
    public const string CalendarTypeIdMustNotBeEmpty = "Calendar Id must have a value";
    public const string CalendarTypeIdMustBeValid = "Calendar Id must have a value";
    public CreateCalendarEventCommandValidator(ICalendarsReadRepository calendarsReadRepository)
    {
        RuleFor(c => c.CalendarId)
            .GreaterThan(0)
            .WithMessage(CalendarTypeIdMustNotBeEmpty)
            .MustAsync(async (id, cancellationToken) =>
            {
                var calendarTypes = await calendarsReadRepository.GetAllCalendars(cancellationToken);
                return calendarTypes.Any(c => c.Id == id);
            })
            .WithMessage(CalendarTypeIdMustBeValid);
    }
}
