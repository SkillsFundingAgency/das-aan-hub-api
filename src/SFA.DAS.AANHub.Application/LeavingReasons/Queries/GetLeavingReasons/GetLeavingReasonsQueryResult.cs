using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;

public class GetLeavingReasonsQueryResult
{
    public IEnumerable<LeavingReasonModel> LeavingReasons { get; set; } = null!;

    public static implicit operator GetLeavingReasonsQueryResult(List<LeavingReason> leavingReasons) => new()
    {
        LeavingReasons = leavingReasons.Select(r => new LeavingReasonModel(r.Id, r.Category, r.Description, r.Ordering))
    };

    public LeavingReasonsProcessed ProcessedResult
    {
        get
        {
            var leavingCategories = new List<LeavingCategory>();
            var categories = LeavingReasons.Select(r => r.Category).Distinct();
            foreach (var category in categories.OrderBy(c => c))
            {
                var leavingReasons = new List<LeavingReasonProcessed>();
                foreach (var r in LeavingReasons.Where(l => l.Category == category))
                {
                    leavingReasons.Add(new LeavingReasonProcessed
                    {
                        Id = r.Id,
                        Description = r.Description,
                        Ordering = r.Ordering
                    });
                }

                leavingCategories.Add(new LeavingCategory
                {
                    Category = category,
                    LeavingReasons = leavingReasons
                });

            }

            return new LeavingReasonsProcessed { LeavingCategories = leavingCategories };
        }
    }
}

public class LeavingReasonsProcessed
{
    public List<LeavingCategory> LeavingCategories { get; set; } = null!;
}

public class LeavingCategory
{
    public string Category { get; set; } = null!;

    public List<LeavingReasonProcessed> LeavingReasons { get; set; } = null!;
}

public class LeavingReasonProcessed
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Ordering { get; set; }
}

public record LeavingReasonModel(int Id, string Category, string Description, int Ordering);