using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface IMembersWriteRepository
{
    void Create(Member member);

    Task<Member?> Get(Guid id);

}