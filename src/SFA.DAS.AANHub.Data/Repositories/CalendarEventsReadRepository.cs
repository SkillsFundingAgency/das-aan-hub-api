using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;
internal class CalendarEventsReadRepository : ICalendarEventsReadRepository
{
    private readonly AanDataContext _aanDataContext;
    public CalendarEventsReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public async Task<CalendarEvent?> GetCalendarEvent(Guid id) => 
        await _aanDataContext
            .CalendarEvents
            .AsNoTracking()
            .Where(m => m.Id == id)
            .Include(x => x.Attendees.Where(a => a.IsActive))
            .ThenInclude(x => x.Member)
            .Include(x => x.EventGuests)
            .SingleOrDefaultAsync();
    
}
