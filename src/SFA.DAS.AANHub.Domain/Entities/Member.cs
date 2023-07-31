using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Domain.Entities;

public class Member
{
    public Guid Id { get; set; }
    public string UserType { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime JoinedDate { get; set; }
    public int? RegionId { get; set; }
    public string? OrganisationName { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public bool IsRegionalChair { get; set; }
    public string FullName { get; set; } = null!;

    public List<MemberProfile> MemberProfiles { get; set; } = new();
    public List<Attendance> Attendances { get; set; } = new();

    public virtual Region? Region { get; set; }

    [JsonIgnore]
    public virtual Apprentice? Apprentice { get; set; }
    [JsonIgnore]
    public virtual Employer? Employer { get; set; }
    [JsonIgnore]
    public virtual Partner? Partner { get; set; }
}