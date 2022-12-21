using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Admins.Commands
{
    public class CreateAdminMemberCommandResponse : CreateMemberCommandResponseBase
    {
        public static implicit operator CreateAdminMemberCommandResponse(Member member) => new()
        {
            MemberId = member.Id,
            Status = member.Status
        };
    }
}
