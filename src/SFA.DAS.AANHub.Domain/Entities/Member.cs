using SFA.DAS.AANHub.Domain.Enums;

namespace SFA.DAS.AANHub.Domain.Entities
{
    public class Member : EntityBase
    {
        public Guid Id { get; set; }
        public MembershipUserType? UserType { get; set; }
        public MembershipStatus? Status { get; set; }
        public MembershipReviewStatus? ReviewStatus { get; set; }
        public string? Information { get; set; }
        public DateTime Joined { get; set; }
        public DateTime? Deleted { get; set; }
        public virtual Admin? Admin { get; set; }
        public virtual Apprentice? Apprentice { get; set; }
        public virtual Employer? Employer { get; set; }
        public virtual Partner? Partner { get; set; }
        public virtual List<MemberRegion>? MemberRegions { get; set; }
        public static List<MemberRegion> GenerateMemberRegions(List<int>? regions, Guid id)
        {
            var memberRegions = new List<MemberRegion>();
            if (regions == null) return memberRegions;

            memberRegions.AddRange(regions.Select(region => new MemberRegion { MemberId = id, RegionId = region }));

            return memberRegions;
        }
    }
}
