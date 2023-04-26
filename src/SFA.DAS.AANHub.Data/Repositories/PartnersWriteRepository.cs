using System.Diagnostics.CodeAnalysis;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class PartnersWriteRepository : IPartnersWriteRepository
{
    private readonly AanDataContext _aanDataContext;

    public PartnersWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public void Create(Partner partner) => _aanDataContext.Partners.Add(partner);
}
