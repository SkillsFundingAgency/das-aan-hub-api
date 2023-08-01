using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;

public class CreateAdminMemberCommand : CreateMemberCommandBase, IRequest<ValidatedResponse<CreateMemberCommandResponse>>
{
    public static implicit operator Member(CreateAdminMemberCommand command) => new()
    {
        Id = command.MemberId,
        UserType = MembershipUserType.Admin,
        Status = MembershipStatus.Live,
        Email = command.Email!,
        FirstName = command.FirstName!,
        LastName = command.LastName!,
        JoinedDate = DateTime.UtcNow,
        RegionId = null,
        OrganisationName = "DFE"
    };
}