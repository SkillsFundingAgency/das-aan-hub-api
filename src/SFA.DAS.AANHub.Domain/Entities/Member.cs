
namespace SFA.DAS.AANHub.Domain.Entities
{
    public class Member : EntityBase
    {
        public Guid Id { get; set; }
        public string? UserType { get; set; }
        public string? Status { get; set; }
        public string? Information { get; set; }
        public string? Organisation { get; set; }
        public DateTime Joined { get; set; }
        public DateTime? Deleted { get; set; }
        public virtual Admin? Admin { get; set; }
        public virtual Apprentice? Apprentice { get; set; }
        public virtual Employer? Employer { get; set; }
        public virtual Partner? Partner { get; set; }
        public virtual List<MemberRegion>? MemberRegions { get; set; }
    }
}
