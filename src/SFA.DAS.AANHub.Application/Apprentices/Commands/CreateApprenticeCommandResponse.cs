using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands
{
    public class CreateApprenticeCommandResponse
    {
        public Guid MemberId { get; set; }
        public string Status { get; set; } = null!;

        public static implicit operator CreateApprenticeCommandResponse(Member member) => new CreateApprenticeCommandResponse()
        {
            MemberId = member.Id,
            Status = member.Status
        };
    }
}
