using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember;

public class CreateEmployerMemberCommand : CreateMemberCommandBase, IRequest<ValidatedResponse<CreateMemberCommandResponse>>
{
    public long AccountId { get; init; }
    public Guid UserRef { get; init; }

    public static implicit operator Member(CreateEmployerMemberCommand command) => new()
    {
        Id = command.MemberId,
        UserType = MembershipUserType.Employer,
        Status = MembershipStatus.Live,
        Email = command.Email!,
        FirstName = command.FirstName!,
        LastName = command.LastName!,
        JoinedDate = command.JoinedDate!.Value,
        RegionId = command.RegionId,
        OrganisationName = command.OrganisationName,
        IsRegionalChair = false,
        Employer = new Employer
        {
            MemberId = command.MemberId,
            AccountId = command.AccountId,
            UserRef = command.UserRef
        },
        MemberProfiles = command.ProfileValues.Select(p => ProfileConverter(p, command.MemberId)).ToList()
    };

    public static MemberProfile ProfileConverter(ProfileValue source, Guid memberId) => new() { MemberId = memberId, ProfileId = source.Id, ProfileValue = source.Value };
}