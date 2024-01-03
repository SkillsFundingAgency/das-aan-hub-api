using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Domain.Entities;

[ExcludeFromCodeCoverage]
public class LeavingReason
{
    public int Id { get; set; }
    public string Category { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Ordering { get; set; }
}