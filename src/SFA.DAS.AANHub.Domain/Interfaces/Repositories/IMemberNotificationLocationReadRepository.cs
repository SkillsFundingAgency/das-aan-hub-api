using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface IMemberNotificationLocationReadRepository
{
    Task<IEnumerable<MemberNotificationLocation>> GetMemberNotificationLocationsByMember(Guid memberId, CancellationToken cancellationToken);
}
