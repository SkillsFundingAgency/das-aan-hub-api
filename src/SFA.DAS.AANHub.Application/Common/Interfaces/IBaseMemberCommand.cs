using SFA.DAS.AANHub.Domain.Enums;

namespace SFA.DAS.AANHub.Application.Common.Interfaces
{
    public interface IBaseMemberCommand
    {
        public Guid Id { get; }
        public MembershipUserType? UserType { get; set; }
        public MembershipReviewStatus? ReviewStatus { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public DateTime Joined { get; set; }
        public int[]? Regions { get; set; }
        public string? Information { get; set; }
    }
}
