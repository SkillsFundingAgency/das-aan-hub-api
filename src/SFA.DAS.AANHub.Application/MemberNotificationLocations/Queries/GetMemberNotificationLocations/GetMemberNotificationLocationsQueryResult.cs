namespace SFA.DAS.AANHub.Application.MemberNotificationLocations.Queries.GetMemberNotificationLocations;

public class GetMemberNotificationLocationsQueryResult
{
    public IEnumerable<MemberNotificationLocationModel> MemberNotificationLocations { get; set; } = Enumerable.Empty<MemberNotificationLocationModel>();
}
