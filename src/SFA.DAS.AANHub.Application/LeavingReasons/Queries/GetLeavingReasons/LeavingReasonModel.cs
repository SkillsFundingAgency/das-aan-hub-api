using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;

public class LeavingReasonModel
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Ordering { get; set; }

    public static implicit operator LeavingReasonModel(LeavingReason source) => new()
    {
        Id = source.Id,
        Description = source.Description,
        Ordering = source.Ordering
    };
}
