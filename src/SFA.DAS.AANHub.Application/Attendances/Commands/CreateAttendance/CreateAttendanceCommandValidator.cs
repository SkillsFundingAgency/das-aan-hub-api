using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;


namespace SFA.DAS.AANHub.Application.Attendances.Commands.CreateAttendance;
public class CreateAttendanceCommandValidator : AbstractValidator<CreateAttendanceCommand>
{
    public const string EventNotFoundMessage = "A calendar event with this ID could not be found";
    public const string EventInPastMessage = "Cannot attend a calendar event that is in the past";
    public const string EventIdNotSuppliedMessage = "A Calendar Event Id was not supplied";

    public CreateAttendanceCommandValidator(ICalendarEventsReadRepository calendarEventsReadRepository, IMembersReadRepository membersReadRepository)
    {
        Include(new RequestedByMemberIdValidator(membersReadRepository));

        RuleFor(c => c.CalendarEventId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(EventIdNotSuppliedMessage)
            .MustAsync(async (eventId, _) =>
            {   
                var tryGetCalendarEvent = await calendarEventsReadRepository.GetCalendarEvent(eventId);
                return tryGetCalendarEvent != null;
            })
            .WithMessage(EventNotFoundMessage)
            .MustAsync(async (eventId, _) =>
            {
                var tryGetCalendarEvent = await calendarEventsReadRepository.GetCalendarEvent(eventId);
                return tryGetCalendarEvent!.StartDate > DateTime.UtcNow;
            })
            .WithMessage(EventInPastMessage);
    }
}
