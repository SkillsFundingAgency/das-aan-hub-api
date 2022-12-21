using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Employers.Commands
{
    public class CreateEmployerMemberCommandResponse : CreateMemberCommandResponseBase
    {
        public static implicit operator CreateEmployerMemberCommandResponse(Member member) => new()
        {
            MemberId = member.Id,
            Status = member.Status.ToString()
        };
    }
}
