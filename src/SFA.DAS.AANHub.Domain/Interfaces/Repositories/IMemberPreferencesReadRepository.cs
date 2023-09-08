using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface IMemberPreferencesReadRepository
{
    Task<IEnumerable<MemberPreference>?> GetMemberPreferencesByMember(Guid memberId, CancellationToken cancellationToken);
}