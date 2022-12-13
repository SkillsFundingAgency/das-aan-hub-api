using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class AuditWriteRepository : IAuditWriteRepository
    {
        private readonly IAanDataContext _aanDataContext;

        public AuditWriteRepository(IAanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public void Create(Audit audit) => _aanDataContext.Audits.Add(audit);
    }
}
