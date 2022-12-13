using SFA.DAS.AANHub.Application.Common.Interfaces;

namespace SFA.DAS.AANHub.Application.Common.Commands
{
    public class BaseMemberCommand : ICreateMemberCommand
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public DateTime Joined { get; set; }
        public int[]? Regions { get; set; }
        public string? Information { get; set; }
    }
}
