using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class RegionsReadRepository : IRegionsReadRepository
    {
        private readonly AanDataContext _aanDataContext;

        public RegionsReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public async Task<List<Region>> GetAllRegions() => await _aanDataContext
                .Regions
                .AsNoTracking()
                .ToListAsync();
    }
}
