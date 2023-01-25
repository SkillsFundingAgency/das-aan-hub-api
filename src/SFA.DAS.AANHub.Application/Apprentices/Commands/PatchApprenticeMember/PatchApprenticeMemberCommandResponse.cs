using SFA.DAS.AANHub.Application.Common.Commands;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands.PatchApprenticeMember
{
    public class PatchApprenticeMemberCommandResponse : PatchMemberCommandResponseBase
    {
        public PatchApprenticeMemberCommandResponse(bool success) : base(success) { }
    }
}
