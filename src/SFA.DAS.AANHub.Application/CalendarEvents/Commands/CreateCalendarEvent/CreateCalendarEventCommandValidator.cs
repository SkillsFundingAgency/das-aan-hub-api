using FluentValidation;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;

public class CreateCalendarEventCommandValidator : AbstractValidator<CreateCalendarEventCommand>
{
    public const string CalendarTypeIdMustNotBeEmpty = "calendarId must have a value";
    public const string CalendarTypeIdMustBeValid = "calendarId must have a valid value";
    public const string EventFormatMustNotBeEmpty = "eventFormat must have a valid value";
    public const string StartDateMustNotBeEmpty = "startDate must have a valid value";
    public const string StartDateMustBeInFuture = "startDate must be a future date";
    public const string StartDateMustBeLessThanEndDate = "startDate must be less than or equal to endDate";
    public const string EndDateMustNotBeEmpty = "endDate must have a valid value";
    public const string EndDateMustBeInFuture = "endDate must be a future date";
    public const string EndDateMustBeLessThanEndDate = "endDate must be greater than or equal to startDate";

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

        RuleFor(c => c.StartDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(StartDateMustNotBeEmpty)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage(StartDateMustBeInFuture)
            .LessThanOrEqualTo(c => c.EndDate)
            .WithMessage(StartDateMustBeLessThanEndDate);

        RuleFor(c => c.EndDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(EndDateMustNotBeEmpty)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage(EndDateMustBeInFuture)
            .GreaterThanOrEqualTo(c => c.StartDate)
            .WithMessage(EndDateMustBeLessThanEndDate);
    }
}
