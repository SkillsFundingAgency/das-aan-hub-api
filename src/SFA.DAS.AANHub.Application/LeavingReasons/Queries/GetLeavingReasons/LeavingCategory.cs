
namespace SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;

public record LeavingCategory
{
    public string Category { get; set; } = null!;

    public List<LeavingReasonProcessed> LeavingReasons { get; set; } = null!;
}