using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IEmployersWriteRepository
    {
        void Create(Employer employer);

        Task<Employer?> GetPatchEmployer(Guid userRef);
    }
}
