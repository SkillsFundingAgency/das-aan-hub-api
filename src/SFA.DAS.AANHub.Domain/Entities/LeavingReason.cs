using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Domain.Entities;

[ExcludeFromCodeCoverage]
public class LeavingReason
{
    public int Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Ordering { get; set; }
}
