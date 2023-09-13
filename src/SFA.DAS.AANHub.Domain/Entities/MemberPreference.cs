using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Domain.Entities;

public class MemberPreference
{
    public long Id { get; set; }
    public Guid MemberId { get; set; }
    public int PreferenceId { get; set; }
    public bool AllowSharing { get; set; }

    [JsonIgnore]
    public virtual Member Member { get; set; } = null!;
}
