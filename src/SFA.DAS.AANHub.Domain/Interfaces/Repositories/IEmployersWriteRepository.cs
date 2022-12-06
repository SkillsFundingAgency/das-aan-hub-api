using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IEmployersWriteRepository
    {
        Task Create(Employer employer);

    }
}
