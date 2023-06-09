using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface ICalendarsReadRepository
{
    Task<List<Calendar>> GetAllCalendars(CancellationToken cancellationToken);
}