using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Attendances.Commands.PutAttendance;

public class PutAttendanceCommandValidator : AbstractValidator<PutAttendanceCommand>
{
    public const string EventIdNotSuppliedMessage = "A Calendar Event Id was not supplied";
    public const string EventNotFoundMessage = "A calendar event with this ID could not be found";
    public const string EventInPastMessage = "Cannot attend a calendar event that is in the past";
    public const string EventNotActiveMessage = "Cannot attend an inactive calendar event";

    public PutAttendanceCommandValidator(ICalendarEventsReadRepository calendarEventsReadRepository, IMembersReadRepository membersReadRepository)
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
            .Must((_) => calendarEvent!.StartDate > DateTime.UtcNow)
            .WithMessage(EventInPastMessage)
            .Must((_) => calendarEvent!.IsActive)
            .WithMessage(EventNotActiveMessage);
    }
}
