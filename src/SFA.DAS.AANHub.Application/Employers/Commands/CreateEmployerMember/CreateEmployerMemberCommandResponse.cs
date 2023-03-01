using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember
{
    public class CreateEmployerMemberCommandResponse
    {
        public Guid MemberId { get; init; }
        public string? Status { get; init; }

        public static implicit operator CreateEmployerMemberCommandResponse(Member member) => new()
        {
            MemberId = member.Id,
            Status = member.Status
        };
    }
}
