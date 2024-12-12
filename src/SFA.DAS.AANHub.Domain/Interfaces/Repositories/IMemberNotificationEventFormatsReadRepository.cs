using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface IMemberNotificationEventFormatsReadRepository
{
    Task<IEnumerable<MemberNotificationEventFormat>> GetMemberNotificationEventFormatsByMember(Guid memberId, CancellationToken cancellationToken);
}
