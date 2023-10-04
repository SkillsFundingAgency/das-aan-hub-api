using System.Diagnostics.CodeAnalysis;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class CalendarEventsWriteRepository : ICalendarEventsWriteRepository
{
    private readonly AanDataContext _aanDataContext;

    public CalendarEventsWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public void Create(CalendarEvent calendarEvent) => _aanDataContext.CalendarEvents.Add(calendarEvent);
}
