using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;
public class AuditReadRepository : IAuditReadRepository
{
    private readonly AanDataContext _aanDataContext;

    public AuditReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;
    public async Task<Audit?> GetLastAttendanceAuditByMemberId(Guid memberId, CancellationToken cancellationToken)
    {
        var query = _aanDataContext.Audits.Where(x => x.ActionedBy == memberId && x.Resource == nameof(Attendance)).OrderByDescending(x => x.AuditTime);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }
}
