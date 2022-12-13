using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IAuditWriteRepository
    {
        void Create(Audit audit);
    }
}
