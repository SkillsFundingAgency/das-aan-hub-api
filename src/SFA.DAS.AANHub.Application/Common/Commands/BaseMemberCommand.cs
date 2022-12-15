using SFA.DAS.AANHub.Application.Common.Interfaces;
using SFA.DAS.AANHub.Domain.Enums;

namespace SFA.DAS.AANHub.Application.Common.Commands
{
    public abstract class BaseMemberCommand : IBaseMemberCommand
    {
        public string? Id { get; set; }
        public MembershipUserTypes? UserType { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public DateTime Joined { get; set; }
        public int[]? Regions { get; set; }
        public string? Information { get; set; }
        public string? Organisation { get; set; }
    }
}
