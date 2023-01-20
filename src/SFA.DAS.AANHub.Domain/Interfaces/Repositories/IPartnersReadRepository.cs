using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IPartnersReadRepository
    {
        Task<Partner?> GetPartnerByUserName(string userName);
    }
}