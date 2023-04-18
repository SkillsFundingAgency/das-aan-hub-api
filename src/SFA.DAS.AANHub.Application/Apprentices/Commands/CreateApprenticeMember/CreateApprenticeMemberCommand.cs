using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember
{
    public class CreateApprenticeMemberCommand : CreateMemberCommandBase, IRequest<ValidatedResponse<CreateApprenticeMemberCommandResponse>>
    {
        public Guid ApprenticeId { get; set; }

        public static implicit operator Member(CreateApprenticeMemberCommand command) => new()
        {
            Id = command.Id,
            UserType = MembershipUserType.Apprentice,
            Joined = command.Joined,
            Information = command.Information,
            Status = MembershipStatus.Live,
            MemberRegions = Member.GenerateMemberRegions(command.Regions, command.Id),
            Apprentice = new Apprentice
            {
                ApprenticeId = command.ApprenticeId,
                MemberId = command.Id,
                Email = command.Email,
                Name = command.Name,
                LastUpdated = DateTime.Now
            }
        };
    }
}