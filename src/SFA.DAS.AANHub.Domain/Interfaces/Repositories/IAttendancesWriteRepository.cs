using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface IAttendancesWriteRepository
{
    void Create(Attendance attendance);
    Task SetActiveStatus(Guid calendarEventId, Guid requestedByMemberId, bool requestedStatus);
    Task<Attendance?> GetAttendance(Guid calendarEventId, Guid requestedByMemberId);
}
