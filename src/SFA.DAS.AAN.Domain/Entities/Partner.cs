
namespace SFA.DAS.AAN.Domain.Entities
{
    public class Partner
    {
        public Guid MemberId { get; set; }
        public long? UKPRN { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}
