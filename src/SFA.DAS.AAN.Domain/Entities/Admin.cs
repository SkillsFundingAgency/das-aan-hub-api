
namespace SFA.DAS.AAN.Domain.Entities
{
    public class Admin
    {
        public Guid MemberId { get; set; }
        public string? Email { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}
