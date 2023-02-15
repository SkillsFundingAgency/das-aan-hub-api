using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember
{
    public class CreateApprenticeMemberCommandResponse
    {
        public Guid MemberId { get; set; }
        public string? Status { get; set; }

        public static implicit operator CreateApprenticeMemberCommandResponse(Member member) => new()
        {
            MemberId = member.Id,
            Status = member.Status
        };
    }
}