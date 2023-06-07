using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface IAttendancesReadRepository
{
    Task<List<Attendance>> GetAttendances(Guid memberId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
}
