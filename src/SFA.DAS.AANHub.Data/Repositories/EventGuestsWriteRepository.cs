using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
public class EventGuestsWriteRepository : IEventGuestsWriteRepository
{
    private readonly AanDataContext _aanDataContext;

    public EventGuestsWriteRepository(AanDataContext aanDataContext)
    {
        _aanDataContext = aanDataContext;
    }

    public void DeleteAll(Guid calendarEventId)
    {
        var guests = _aanDataContext.EventGuests.Where(g => g.CalendarEventId == calendarEventId);
        foreach (var guest in guests)
        {
            _aanDataContext.EventGuests.Remove(guest);
        }
    }

    public void CreateGuests(List<EventGuest> guests)
    {
        _aanDataContext.EventGuests.AddRange(guests);
    }
}
