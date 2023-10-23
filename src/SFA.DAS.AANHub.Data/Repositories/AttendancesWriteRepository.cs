using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class AttendancesWriteRepository : IAttendancesWriteRepository
{
    private readonly AanDataContext _aanDataContext;

    public AttendancesWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public void Create(Attendance attendance) => _aanDataContext.Attendances.Add(attendance);

    public async Task<Attendance?> GetAttendance(Guid calendarEventId, Guid memberId) =>
        await _aanDataContext.Attendances.Where(a => a.CalendarEventId == calendarEventId)
                                         .Where(a => a.MemberId == memberId)
                                         .SingleOrDefaultAsync();

    public async Task<List<Attendance>> GetAttendancesByEventId(Guid calendarEventId, CancellationToken cancellationToken)
    {
        var query = _aanDataContext
            .Attendances
            .Where(a =>
                a.IsAttending &&
                a.CalendarEventId == calendarEventId);
        return await query.ToListAsync(cancellationToken);
    }
}
