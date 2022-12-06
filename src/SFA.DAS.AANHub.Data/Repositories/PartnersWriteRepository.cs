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

        public async Task Create(Partner partner)
        {
            _aanDataContext.Partners.Add(partner);
            await _aanDataContext.SaveChangesAsync();
        }
    }
}
