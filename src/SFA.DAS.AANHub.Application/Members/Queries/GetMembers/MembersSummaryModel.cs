using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMembers;

public class MembersSummaryModel
{
    public Guid MemberId { get; set; }
    public string FullName { get; set; } = null!;
    public int? RegionId { get; set; }
    public string? RegionName { get; set; }
    public UserType UserType { get; set; }
    public bool? IsRegionalChair { get; set; }
    public DateTime JoinedDate { get; set; }

    public static implicit operator MembersSummaryModel(MemberSummary summary) => new()
    {
        MemberId = summary.MemberId,
        FullName = summary.FullName,
        RegionId = summary.RegionId,
        RegionName = summary.RegionName,
        UserType = summary.UserType,
        IsRegionalChair = summary.IsRegionalChair,
        JoinedDate = summary.JoinedDate,
    };
}