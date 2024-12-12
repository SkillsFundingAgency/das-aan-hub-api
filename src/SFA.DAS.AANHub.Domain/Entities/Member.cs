using System.Text.Json.Serialization;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Domain.Entities;

public class Member
{
    public Guid Id { get; set; }
    public UserType UserType { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime JoinedDate { get; set; }
    public int? RegionId { get; set; }
    public string? OrganisationName { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public bool? IsRegionalChair { get; set; }
    public string FullName { get; set; } = null!;
    public bool ReceiveNotifications { get; set; }
    public virtual List<MemberProfile> MemberProfiles { get; set; } = new();
    public virtual List<MemberPreference> MemberPreferences { get; set; } = new();
    public virtual List<MemberNotificationEventFormat>? MemberNotificationEventFormats { get; set; }
    public virtual List<MemberNotificationLocation>? MemberNotificationLocations { get; set; }
    public virtual List<MemberLeavingReason> MemberLeavingReasons { get; set; } = new();
    public virtual List<Attendance> Attendances { get; set; } = new();
    public virtual List<Audit> Audits { get; set; } = new();
    public virtual Region? Region { get; set; }

    [JsonIgnore]
    public virtual Apprentice? Apprentice { get; set; }
    [JsonIgnore]
    public virtual Employer? Employer { get; set; }
}