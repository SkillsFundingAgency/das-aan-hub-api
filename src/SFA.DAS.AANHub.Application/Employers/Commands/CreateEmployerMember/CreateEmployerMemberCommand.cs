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
    public string Organisation { get; set; } = null!;

    public static implicit operator Member(CreateEmployerMemberCommand command) => new()
    {
        Id = command.Id,
        UserType = MembershipUserType.Employer,
        Status = MembershipStatus.Live,
        Email = command.Email!,
        FirstName = command.FirstName!,
        LastName = command.LastName!,
        JoinedDate = command.JoinedDate!.Value,
        RegionId = command.RegionId,
        OrganisationName = command.OrganisationName,
        Employer = new Employer
        {
            MemberId = command.Id,
            AccountId = command.AccountId,
            UserRef = command.UserRef
        }
    };
}