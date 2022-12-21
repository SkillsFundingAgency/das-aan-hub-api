using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IAdminsReadRepository
    {
        Task<Admin?> GetAdminByUserName(string userName);
    }
}
