using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IMembersWriteRepository
    {
        Task Create(Member member);

    }
}
