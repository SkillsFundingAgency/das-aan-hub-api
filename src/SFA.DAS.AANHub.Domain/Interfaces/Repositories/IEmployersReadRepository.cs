using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IEmployersReadRepository
    {
        Task<Employer?> GetEmployerByAccountIdAndUserId(long accountId, long userId);

        Task<Employer?> GetEmployerByUserId(long userId);
    }
}
