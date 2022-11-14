
using SFA.DAS.AAN.Domain.Entities;


namespace SFA.DAS.AAN.Domain.Interfaces
{
    public interface IPartnersContext : IEntityContext<Partner>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
