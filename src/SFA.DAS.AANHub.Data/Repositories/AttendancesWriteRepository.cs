using System.Diagnostics.CodeAnalysis;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
public class AttendancesWriteRepository : IAttendancesWriteRepository
{
    private readonly AanDataContext _aanDataContext;

    public AttendancesWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public void Create(Attendance attendance) => _aanDataContext.Attendances.Add(attendance);

    public void SetActiveStatus(Guid calendarEventId, Guid requestedByMemberId, bool requestedStatus)
    {
        var existingAttendance = _aanDataContext.Attendances.Where(a => a.CalendarEventId == calendarEventId)
                                                            .Where(a => a.MemberId == requestedByMemberId)
                                                            .Single();

        existingAttendance.IsActive = requestedStatus;
    }
}
