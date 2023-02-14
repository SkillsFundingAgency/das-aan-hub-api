namespace SFA.DAS.AANHub.Domain.Entities
{
    public class Apprentice
    {
        public Guid MemberId { get; set; }
        public long ApprenticeId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime? LastUpdated { get; set; }
        public Member Member { get; set; } = null!;
    }
}