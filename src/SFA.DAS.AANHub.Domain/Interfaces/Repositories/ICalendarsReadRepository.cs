using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface ICalendarsReadRepository
    {
        Task<Calendar?> GetCalendar(int calendarId);
        Task<string?> GetCalendarName(int calendarId);
    }
}
