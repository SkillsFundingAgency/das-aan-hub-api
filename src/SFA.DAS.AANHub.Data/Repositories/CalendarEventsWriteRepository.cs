using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class CalendarEventsWriteRepository : ICalendarEventsWriteRepository
{
    private readonly AanDataContext _aanDataContext;

    public CalendarEventsWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public void Create(CalendarEvent calendarEvent) => _aanDataContext.CalendarEvents.Add(calendarEvent);

    public async Task<CalendarEvent> GetCalendarEvent(Guid id) =>
        await _aanDataContext
            .CalendarEvents
            .Where(m => m.Id == id)
            .SingleAsync();

}
