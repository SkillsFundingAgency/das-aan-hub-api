namespace SFA.DAS.AANHub.Domain.Entities;

public class Partner
{
    public Guid MemberId { get; set; }
    public string UserName { get; set; } = null!;
    public Member Member { get; set; } = null!;
}