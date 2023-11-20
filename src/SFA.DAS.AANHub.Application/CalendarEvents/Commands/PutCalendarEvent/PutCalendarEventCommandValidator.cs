using FluentValidation;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
public class PutCalendarEventCommandValidator : AbstractValidator<PutCalendarEventCommand>
{
    public const string EventIdNotSuppliedMessage = "A Calendar Event Id was not supplied";
    public const string EventNotFoundMessage = "A calendar event with this ID could not be found";
    public const string EventInPastMessage = "Cannot amend a calendar event that is in the past";
    public const string EventNotActiveMessage = "Cannot amend a calendar event that has been cancelled";

    public PutCalendarEventCommandValidator(
        ICalendarsReadRepository calendarsReadRepository,
        IRegionsReadRepository regionsReadRepository,
        IMembersReadRepository membersReadRepository,
        ICalendarEventsReadRepository calendarEventsReadRepository)
    {
        Include(new CalendarEventCommandBase.CalendarEventCommandBaseValidator(calendarsReadRepository, regionsReadRepository, membersReadRepository));

        CalendarEvent? calendarEvent = null;

        RuleFor(c => c.CalendarEventId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(EventIdNotSuppliedMessage)
            .MustAsync(async (eventId, _) =>
            {
                calendarEvent = await calendarEventsReadRepository.GetCalendarEvent(eventId);
                return calendarEvent != null;
            })
            .WithMessage(EventNotFoundMessage)
            .Must((_) => calendarEvent!.IsActive)
            .WithMessage(EventNotActiveMessage)
            .Must((_) => calendarEvent!.StartDate > DateTime.UtcNow)
            .WithMessage(EventInPastMessage);
    }
}
