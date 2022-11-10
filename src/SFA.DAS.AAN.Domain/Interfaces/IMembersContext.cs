
using SFA.DAS.AAN.Domain.Entities;


namespace SFA.DAS.AAN.Domain.Interfaces
{
    public interface IMembersContext : IEntityContext<Member>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
