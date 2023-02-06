namespace SFA.DAS.AANHub.Domain.Entities
{
    public class Admin
    {
        public Guid MemberId { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public DateTime LastUpdated { get; set; }
    }
}