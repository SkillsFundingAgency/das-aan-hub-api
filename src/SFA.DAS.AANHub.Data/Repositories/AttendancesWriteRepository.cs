using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
public class AttendancesWriteRepository : IAttendancesWriteRepository
{
    private readonly AanDataContext _aanDataContext;

    public AttendancesWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public void Create(Attendance attendance) => _aanDataContext.Attendances.Add(attendance);

    public async Task<Attendance?> GetAttendance(Guid calendarEventId, Guid memberId) =>
        await _aanDataContext.Attendances.Where(a => a.CalendarEventId == calendarEventId)
                                         .Where(a => a.MemberId == memberId)
                                         .SingleOrDefaultAsync();
}
