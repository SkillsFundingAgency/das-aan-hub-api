using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IAdminsWriteRepository
    {
        void Create(Admin admin);
        Task<Admin?> GetPatchAdmin(string userName);
    }
}