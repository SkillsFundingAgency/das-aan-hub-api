using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface IAttendancesWriteRepository
{
    void Create(Attendance attendance);
    void SetActiveStatus(Guid calendarEventId, Guid requestedByMemberId, bool requestedStatus);
}
