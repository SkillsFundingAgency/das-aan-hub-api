namespace SFA.DAS.AANHub.Domain.Entities;

public class Apprentice
{
    public Guid MemberId { get; set; }
    public Guid ApprenticeId { get; set; }
    public Member Member { get; set; } = null!;
}