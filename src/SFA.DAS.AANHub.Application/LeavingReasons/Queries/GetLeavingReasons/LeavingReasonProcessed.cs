namespace SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;

public class LeavingReasonProcessed
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Ordering { get; set; }
}