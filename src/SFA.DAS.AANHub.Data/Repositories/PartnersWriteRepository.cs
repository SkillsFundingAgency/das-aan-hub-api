using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class PartnersWriteRepository : IPartnersWriteRepository
    {
        private readonly AanDataContext _aanDataContext;

        public PartnersWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public void Create(Partner partner) => _aanDataContext.Partners.Add(partner);

        public async Task<Partner?> GetPatchPartner(string userName) => await _aanDataContext
            .Partners
            .Where(a => a.UserName == userName)
            .SingleOrDefaultAsync();
    }
}
