using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Partners.Commands
{
    public class CreatePartnerMemberCommandResponse : CreateMemberCommandResponseBase
    {
        public static implicit operator CreatePartnerMemberCommandResponse(Member member) => new()
        {
            MemberId = member.Id,
            Status = member.Status
        };
    }
}