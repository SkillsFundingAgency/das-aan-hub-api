namespace SFA.DAS.AANHub.Domain.Entities
{
    public class Partner
    {
        public Guid MemberId { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Organisation { get; set; } = null!;
        public DateTime? LastUpdated { get; set; }
        public Member Member { get; set; } = null!;
    }
}