using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IPartnersWriteRepository
    {
        void Create(Partner partner);

        Task<Partner?> GetPatchPartner(string userName);
    }
}
