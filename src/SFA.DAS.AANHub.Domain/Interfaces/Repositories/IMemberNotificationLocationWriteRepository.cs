using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;

public interface IMemberNotificationLocationWriteRepository
{
    void UpdateMemberNotificationLocations(List<MemberNotificationLocation> locations, CancellationToken cancellationToken);
    void DeleteMemberNotificationLocations(List<MemberNotificationLocation> locations, CancellationToken cancellationToken);
}