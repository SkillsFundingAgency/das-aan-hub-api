using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Partners.Commands.CreatePartnerMember;

public class CreatePartnerMemberCommand : CreateMemberCommandBase, IRequest<ValidatedResponse<CreateMemberCommandResponse>>
{
    public string UserName { get; set; } = null!;
    public string Organisation { get; set; } = null!;

    public static implicit operator Member(CreatePartnerMemberCommand command) => new()
    {
        Id = command.Id,
        UserType = MembershipUserType.Partner,
        Status = MembershipStatus.Live,
        Email = command.Email!,
        FirstName = command.FirstName!,
        LastName = command.LastName!,
        Joined = command.Joined!.Value,
        RegionId = command.RegionId,
        OrganisationName = command.OrganisationName,
        Partner = new Partner
        {
            MemberId = command.Id,
            UserName = command.UserName
        }
    };
}