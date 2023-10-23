using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.DeleteCalendarEvent;

public class DeleteCalendarEventCommandValidator : AbstractValidator<DeleteCalendarEventCommand>
{
    public const string EventIdNotSuppliedMessage = "A Calendar Event Id was not supplied";
    public const string EventNotFoundMessage = "A calendar event with this ID could not be found";
    public const string EventInPastMessage = "Cannot amend a calendar event that is in the past";
    public const string EventNotActiveMessage = "Cannot amend a cancelled calendar event";

    public DeleteCalendarEventCommandValidator(ICalendarEventsReadRepository calendarEventsReadRepository, IMembersReadRepository membersReadRepository)
    {
        Include(new RequestedByMemberIdValidator(membersReadRepository));

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
