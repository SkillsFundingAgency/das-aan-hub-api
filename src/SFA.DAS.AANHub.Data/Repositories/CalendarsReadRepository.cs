using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
public class CalendarsReadRepository : ICalendarsReadRepository
{
    private readonly AanDataContext _aanDataContext;
    public CalendarsReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public async Task<Calendar?> GetCalendar(int calendarId) => await _aanDataContext
                                                                    .Calendars
                                                                    .AsNoTracking()
                                                                    .SingleOrDefaultAsync(m => m.Id == calendarId);

    public async Task<string?> GetCalendarName(int calendarId)
    {
        var calendar = await GetCalendar(calendarId);
        return calendar?.CalendarName;
    }
}
