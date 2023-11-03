using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface ICalendarEventsWriteRepository
{
    void Create(CalendarEvent calendarEvent);
    Task<CalendarEvent> GetCalendarEvent(Guid id);
}
