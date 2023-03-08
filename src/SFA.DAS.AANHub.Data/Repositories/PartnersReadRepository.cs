using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class PartnersReadRepository : IPartnersReadRepository
    {
        private readonly AanDataContext _aanDataContext;

        public PartnersReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public async Task<Partner?> GetPartnerByUserName(string userName) => await _aanDataContext
            .Partners
            .AsNoTracking().Where(a => a.UserName == userName).Include(x => x.Member)
            .SingleOrDefaultAsync();
    }
}