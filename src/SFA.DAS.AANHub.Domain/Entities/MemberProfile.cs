using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Domain.Entities;

public class MemberProfile
{
    public long Id { get; set; }
    public Guid MemberId { get; set; }
    public int ProfileId { get; set; }
    public string ProfileValue { get; set; } = null!;

    [JsonIgnore]
    public virtual Member Member { get; set; } = null!;

    [JsonIgnore]
    public virtual Profile Profile { get; set; } = null!;
}
