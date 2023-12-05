using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Domain.Entities;

[Keyless]
public class MemberSummary
{
    public Guid MemberId { get; set; }
    public string FullName { get; set; } = null!;
    public int? RegionId { get; set; }
    public string? RegionName { get; set; }
    public UserType UserType { get; set; }
    public bool? IsRegionalChair { get; set; }
    public DateTime JoinedDate { get; set; }
    public int TotalCount { get; set; }
}