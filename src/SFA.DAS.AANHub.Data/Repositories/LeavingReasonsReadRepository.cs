using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class LeavingReasonsReadRepository : ILeavingReasonsReadRepository
{
    private readonly AanDataContext _aanDataContext;

    public LeavingReasonsReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public async Task<List<LeavingReason>> GetAllLeavingReasons(CancellationToken cancellationToken) =>
        await _aanDataContext
            .LeavingReasons
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}