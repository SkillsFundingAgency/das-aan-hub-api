using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Partners.Commands.CreatePartnerMember
{
    public class CreatePartnerMemberCommand : CreateMemberCommandBase, IRequest<ValidatedResponse<CreateMemberCommandResponse>>
    {
        public string UserName { get; set; } = null!;
        public string Organisation { get; set; } = null!;

        public static implicit operator Member(CreatePartnerMemberCommand command) => new()
        {
            Id = command.Id,
            UserType = MembershipUserType.Partner,
            Joined = command.Joined,
            Information = command.Information,
            Status = MembershipStatus.Live,
            RegionId = command.RegionId,
            Partner = new Partner
            {
                MemberId = command.Id,
                Email = command.Email,
                UserName = command.UserName,
                Name = command.Name,
                Organisation = command.Organisation,
                LastUpdated = DateTime.Now
            }
        };
    }
}