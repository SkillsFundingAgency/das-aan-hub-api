using SFA.DAS.AANHub.Domain.Enums;

namespace SFA.DAS.AANHub.Application.Common
{
    public interface ICreateMemberCommand
    {
        long ApprenticeId { get; set; }
        MembershipUserTypes? UserType { get; set; }
        DateTime Joined { get; set; }
        int? Region { get; set; }
        string? Information { get; set; }
    }

}
