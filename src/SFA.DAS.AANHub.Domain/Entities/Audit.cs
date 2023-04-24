using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Domain.Entities;

[ExcludeFromCodeCoverage]
public class Audit
{
    public long Id { get; set; }
    public DateTime AuditTime { get; set; }
    public Guid ActionedBy { get; set; }
    public string Action { get; set; } = null!;
    public string Resource { get; set; } = null!;
    public string? Before { get; set; }
    public string? After { get; set; }
}
