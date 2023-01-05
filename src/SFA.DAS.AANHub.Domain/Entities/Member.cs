using SFA.DAS.AANHub.Domain.Enums;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Domain.Entities
{
    public class Member : EntityBase
    {
        public Guid Id { get; set; }
        public MembershipUserType? UserType { get; set; }
        public string? Status { get; set; }
        public MembershipReviewStatus? ReviewStatus { get; set; }
        public string? Information { get; set; }
        public DateTime Joined { get; set; }
        public DateTime? Deleted { get; set; }
        public virtual Admin? Admin { get; set; }
        public virtual Apprentice? Apprentice { get; set; }
        public virtual Employer? Employer { get; set; }
        public virtual Partner? Partner { get; set; }
        public virtual List<MemberRegion>? MemberRegions { get; set; }
    }
}
