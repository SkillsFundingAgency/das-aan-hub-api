namespace SFA.DAS.AANHub.Domain.Entities;

public class Admin
{
    public Guid MemberId { get; set; }
    public string UserName { get; set; } = null!;
    public Member Member { get; set; } = null!;
}