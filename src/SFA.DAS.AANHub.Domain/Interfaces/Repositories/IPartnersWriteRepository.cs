using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IPartnersWriteRepository
    {
        Task Create(Partner partner);
    }
}
