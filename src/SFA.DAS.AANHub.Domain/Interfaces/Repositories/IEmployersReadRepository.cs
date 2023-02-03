using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IEmployersReadRepository
    {
        Task<Employer?> GetEmployerByUserRef(Guid userRef);
    }
}