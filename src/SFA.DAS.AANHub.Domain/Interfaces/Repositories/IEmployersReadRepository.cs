using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IEmployersReadRepository
    {
        Task<Employer?> GetEmployerByUserRef(Guid userRef);
        Task<Employer?> GetEmployerByMemberId(Guid memberId, CancellationToken cancellationToken);
    }
}