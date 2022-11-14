
using SFA.DAS.AAN.Domain.Entities;


namespace SFA.DAS.AAN.Domain.Interfaces
{
    public interface IEmployersContext : IEntityContext<Employer>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
