
using SFA.DAS.AAN.Domain.Entities;


namespace SFA.DAS.AAN.Domain.Interfaces
{
    public interface IAdminsContext : IEntityContext<Admin>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
