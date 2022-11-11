using SFA.DAS.AAN.Application.Interfaces;
using SFA.DAS.AAN.Domain.Entities.Audit;
using SFA.DAS.AAN.Domain.Enums;

namespace SFA.DAS.AAN.Application.Services
{
    public class AuditLogService<T> : IAuditLogService<T> where T : class
    {
        public AuditData BuildAuditLog(AuditMetadata metadata, T? originalData, T? newData)
        {
            var auditData = new AuditData
            {
                Action = metadata.Action.ToString(),
                ActionedBy = metadata.ActionedBy,
                AuditTime = DateTime.Now,
                Resource = metadata.Resource,
                EntityId = metadata.EntityId,
            };

            if (metadata.Action == Actions.Update)
            {
                //TODO: Implement method to extract the property data edits
            }

            return auditData;
        }

    }
}
