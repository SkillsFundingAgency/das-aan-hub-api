using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Enums;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands
{
    public class CreateApprenticesCommand : IRequest<CreateApprenticeCommandResponse>, ICreateMemberCommand
    {
        public long ApprenticeId { get; set; }
        public DateTime Joined { get; set; }
        public int? Region { get; set; }
        public string? Information { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;

        public static implicit operator Member(CreateApprenticesCommand command) => new Member()
        {
            Id = Guid.NewGuid(),
            UserType = MembershipUserTypes.Apprentice.ToString(),
            Joined = command.Joined,
            RegionId = command.Region,
            Information = command.Information,
            Status = MembershipStatuses.Live.ToString(),
            Apprentice = new Apprentice()
            {
                ApprenticeId = command.ApprenticeId,
                Email = null,
                Name = null,
                IsActive = true
            }
        };
    }
}
