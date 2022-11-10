
using SFA.DAS.AAN.Domain.Entities;


namespace SFA.DAS.AAN.Domain.Interfaces
{
    public interface IApprenticesContext : IEntityContext<Apprentice>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
