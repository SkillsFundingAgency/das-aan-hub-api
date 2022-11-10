
namespace SFA.DAS.AAN.Domain.Entities
{
    public class Apprentice
    {
        public Guid MemberId { get; set; }
        public long ApprenticeId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}
