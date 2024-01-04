
namespace SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;

public record LeavingCategory(string Category, IEnumerable<LeavingReasonModel> LeavingReasons);
