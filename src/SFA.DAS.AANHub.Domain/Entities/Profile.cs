using System.Text.Json.Serialization;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Domain.Entities;

public class Profile
{
    public int Id { get; set; }
    public UserType UserType { get; set; }
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!;
    public int Ordering { get; set; }
    public int? PreferenceId { get; set; }

    [JsonIgnore]
    public virtual Preference Preference { get; set; } = null!;
}
