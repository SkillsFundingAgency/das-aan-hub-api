using SFA.DAS.AAN.Domain.Entities.Audit;

namespace SFA.DAS.AAN.Application.Interfaces
{
    public interface IAuditLogService<in T>
    {
        AuditData BuildAuditLog(AuditMetadata metadata, T? originalData, T? newData);
    }
}
