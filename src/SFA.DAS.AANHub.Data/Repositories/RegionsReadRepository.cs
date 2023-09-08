using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class RegionsReadRepository : IRegionsReadRepository
{
    private readonly AanDataContext _aanDataContext;

    public RegionsReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public async Task<List<Region>> GetAllRegions(CancellationToken cancellationToken) => await _aanDataContext
            .Regions
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<Region> GetRegionById(int Id, CancellationToken cancellationToken) => await _aanDataContext
            .Regions
            .AsNoTracking()
            .Where(r => r.Id == Id)
            .SingleAsync(cancellationToken);
}