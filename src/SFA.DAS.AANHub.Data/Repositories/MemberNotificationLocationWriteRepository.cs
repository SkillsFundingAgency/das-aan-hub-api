using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

internal class MemberNotificationLocationWriteRepository(AanDataContext aanDataContext)
    : IMemberNotificationLocationWriteRepository
{
    public void UpdateMemberNotificationLocations(List<MemberNotificationLocation> locations, CancellationToken cancellationToken)
    {
        aanDataContext.MemberNotificationLocations.UpdateRange(locations);
    }

    public void DeleteMemberNotificationLocations(List<MemberNotificationLocation> locations, CancellationToken cancellationToken)
    {
        aanDataContext.MemberNotificationLocations.RemoveRange(locations);
    }
}