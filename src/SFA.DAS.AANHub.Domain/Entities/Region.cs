using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Domain.Entities;

public class Region
{
    public int Id { get; set; }
    public string? Area { get; set; }
    public int Ordering { get; set; }

    [JsonIgnore]
    public virtual List<Member> Members { get; set; } = new();
}
