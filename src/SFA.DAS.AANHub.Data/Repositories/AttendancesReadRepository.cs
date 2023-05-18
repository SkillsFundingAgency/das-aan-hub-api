using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Data.Configuration;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
public class AttendancesReadRepository : IAttendancesReadRepository
{
    private readonly AanDataContext _aanDataContext;

    public AttendancesReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public async Task<Attendance?> GetAttendance(Guid calendarEventId, Guid memberId) =>
        await _aanDataContext.Attendances.Where(a => a.CalendarEventId == calendarEventId)
                                         .Where(a => a.MemberId == memberId)
                                         .SingleOrDefaultAsync();
}
