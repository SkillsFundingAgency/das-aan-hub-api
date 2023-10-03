using FluentValidation;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;

public class CreateCalendarEventCommandValidator : AbstractValidator<CreateCalendarEventCommand>
{
    public const string CalendarTypeIdMustNotBeEmpty = "calendarId must have a value";
    public const string CalendarTypeIdMustBeValid = "calendarId must have a valid value";
    public const string EventFormatMustNotBeEmpty = "eventFormat must have a valid value";
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

        RuleFor(c => c.EventFormat)
            .NotEmpty()
            .WithMessage(EventFormatMustNotBeEmpty);

    }
}
