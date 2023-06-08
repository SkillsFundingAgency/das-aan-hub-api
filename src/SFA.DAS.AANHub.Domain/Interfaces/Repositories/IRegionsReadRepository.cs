using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface IRegionsReadRepository
{
    Task<List<Region>> GetAllRegions(CancellationToken cancellationToken);
}
