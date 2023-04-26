namespace SFA.DAS.AANHub.Domain.Entities;

public class Employer
{
    public Guid MemberId { get; set; }
    public long AccountId { get; set; }
    public Guid UserRef { get; set; }
    public Member Member { get; set; } = null!;
}