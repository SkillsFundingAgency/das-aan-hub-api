using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Employers.Commands
{
    public class CreateEmployerMemberResponse
    {
        public Guid MemberId { get; set; }
        public string? Status { get; set; }

        public static implicit operator CreateEmployerMemberResponse(Member member) => new()
        {
            MemberId = member.Id,
            Status = member.Status.ToString()
        };
    }
}
