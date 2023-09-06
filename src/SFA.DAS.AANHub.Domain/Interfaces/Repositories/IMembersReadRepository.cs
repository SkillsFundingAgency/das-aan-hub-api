using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface IMembersReadRepository
{
    Task<Member?> GetMember(Guid id);

    Task<Member?> GetMemberByEmail(string email);
}
