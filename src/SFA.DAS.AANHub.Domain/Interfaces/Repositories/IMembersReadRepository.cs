using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Models;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface IMembersReadRepository
{
    Task<Member?> GetMember(Guid id);

    Task<Member?> GetMemberByEmail(string Email);

    Task<List<MemberSummary>> GetMembers(GetMembersOptions options, CancellationToken cancellationToken);

    Task<List<Member>> GetMembers(List<Guid> memberIds, CancellationToken cancellationToken);
}
