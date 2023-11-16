using FluentValidation;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
public class CalendarEventCommandBaseValidator : AbstractValidator<CreateCalendarEventCommand>
{
    public CalendarEventCommandBaseValidator(
        ICalendarsReadRepository calendarsReadRepository,
        IRegionsReadRepository regionsReadRepository,
        IMembersReadRepository membersReadRepository)
    {
        Include(new CalendarEventCommandBase.CalendarEventCommandBaseValidator(calendarsReadRepository, regionsReadRepository, membersReadRepository));
    }
}
