using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMembers;

public class MembersSummaryModel
{
    public Guid MemberId { get; set; }
    public string? FullName { get; set; } = null!;
    public int? RegionId { get; set; } = null!;
    public string? RegionName { get; set; } = null!;
    public string UserType { get; set; } = "";
    public bool? IsRegionalChair { get; set; } = null!;
    public DateTime JoinedDate { get; set; }

    public static implicit operator MembersSummaryModel(MembersSummary summary) => new()
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