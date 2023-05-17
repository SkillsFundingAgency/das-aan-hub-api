using SFA.DAS.AANHub.Domain.Models;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface ICalendarEventsReadRepository
{
    Task<List<CalendarEventModel>> GetCalendarEvents(Guid memberId);
    Task<CalendarEvent?> GetCalendarEvent(Guid id);
}
