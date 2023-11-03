using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface IAttendancesWriteRepository
{
    void Create(Attendance attendance);
    Task<Attendance?> GetAttendance(Guid calendarEventId, Guid memberId);
    Task<List<Attendance>> GetAttendancesByEventId(Guid calendarEventId, CancellationToken cancellationToken);
}
