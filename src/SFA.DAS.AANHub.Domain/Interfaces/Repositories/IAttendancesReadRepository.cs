using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface IAttendancesReadRepository
{
    Task<Attendance?> GetAttendance(Guid calendarEventId, Guid memberId);
}
