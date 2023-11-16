using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class AttendancesReadRepository : IAttendancesReadRepository
{
    private readonly AanDataContext _aanDataContext;

    public AttendancesReadRepository(AanDataContext aanDataContext)
    {
        _aanDataContext = aanDataContext;
    }

    public async Task<List<Attendance>> GetAttendances(Guid memberId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
    {
        var query = _aanDataContext
            .Attendances
            .AsNoTracking()
            .Include(a => a.CalendarEvent)
            .Where(a =>
                a.IsAttending &&
                a.MemberId == memberId &&
                a.CalendarEvent.IsActive &&
                a.CalendarEvent.StartDate >= fromDate &&
                a.CalendarEvent.StartDate < toDate)
            .OrderBy(a => a.CalendarEvent.StartDate);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<Attendance>> GetAttendancesByEventId(Guid calendarEventId, CancellationToken cancellationToken)
    {
        var query = _aanDataContext
            .Attendances
            .AsNoTracking()
            .Where(a =>
                a.IsAttending &&
                a.CalendarEventId == calendarEventId);
        return await query.ToListAsync(cancellationToken);
    }
}
