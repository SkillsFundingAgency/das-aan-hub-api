using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IAuditReadRepository
    {
        Task<Audit?> GetLastAttendanceAuditByMemberId(Guid memberId, CancellationToken cancellationToken);
    }
}