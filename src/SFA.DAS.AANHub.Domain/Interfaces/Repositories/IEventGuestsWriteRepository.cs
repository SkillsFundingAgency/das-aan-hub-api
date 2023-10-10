using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;
public interface IEventGuestsWriteRepository
{
    void DeleteAll(Guid calendarEventId);
    void CreateGuests(List<EventGuest> guests);
}
