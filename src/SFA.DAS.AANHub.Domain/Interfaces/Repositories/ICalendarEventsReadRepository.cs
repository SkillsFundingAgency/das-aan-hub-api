using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Models;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface ICalendarEventsReadRepository
{
    Task<List<CalendarEventSummary>> GetCalendarEvents(Guid memberId, CancellationToken cancellationToken);
    Task<CalendarEvent?> GetCalendarEvent(Guid id);
}
