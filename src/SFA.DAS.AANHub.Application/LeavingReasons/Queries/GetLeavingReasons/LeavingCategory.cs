
namespace SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;

public record LeavingCategory
{
    public string Category { get; set; } = null!;

    public List<LeavingReasonModel> LeavingReasons { get; set; } = null!;
}