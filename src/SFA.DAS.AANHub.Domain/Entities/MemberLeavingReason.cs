namespace SFA.DAS.AANHub.Domain.Entities;
public class MemberLeavingReason
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public int LeavingReasonId { get; set; }
}
