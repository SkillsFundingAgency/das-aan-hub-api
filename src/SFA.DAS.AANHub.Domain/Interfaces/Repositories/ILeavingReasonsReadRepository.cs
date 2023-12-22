using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface ILeavingReasonsReadRepository
{
    Task<List<LeavingReason>> GetAllLeavingReasons(CancellationToken cancellationToken);
}