
namespace SFA.DAS.AANHub.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; set; }
        public string? UserType { get; set; }
        public string? Status { get; set; }
        public string? Information { get; set; }
        public int? RegionId { get; set; }
        public string? Organisation { get; set; }
        public DateTime Joined { get; set; }

        public virtual Apprentice? Apprentice { get; set; }
    }
}
