
namespace SFA.DAS.AAN.Domain.Entities
{
    public class Employer
    {
        public Guid MemberId { get; set; }
        public long AccountId { get; set; }
        public long? UserId { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}
