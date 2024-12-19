using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.MemberNotificationLocations.Queries.GetMemberNotificationSettings;

public class MemberNotificationLocationModel
{
    public long Id { get; set; }
    public Guid MemberId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Radius { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public static implicit operator MemberNotificationLocationModel(MemberNotificationLocation source) => new()
    {
        Id = source.Id,
        MemberId = source.MemberId,
        Name = source.Name,
        Radius = source.Radius,
        Latitude = source.Latitude,
        Longitude = source.Longitude
    };
}