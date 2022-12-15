using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Employers.Commands
{
    public class CreateEmployersResponse
    {
        public Guid MemberId { get; set; }
        public string? Status { get; set; }

        public static implicit operator CreateEmployersResponse(Member member) => new()
        {
            MemberId = member.Id,
            Status = member.Status.ToString()
        };
    }
}
