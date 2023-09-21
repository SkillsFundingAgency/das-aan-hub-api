using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface IMemberProfilesReadRepository
{
    Task<IEnumerable<MemberProfile>> GetMemberProfilesByMember(Guid memberId, CancellationToken cancellationToken);
}
