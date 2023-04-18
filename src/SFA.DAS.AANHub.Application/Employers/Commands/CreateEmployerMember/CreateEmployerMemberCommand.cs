using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember
{
    public class CreateEmployerMemberCommand : CreateMemberCommandBase, IRequest<ValidatedResponse<CreateMemberCommandResponse>>
    {
        public long AccountId { get; init; }
        public Guid UserRef { get; init; }
        public string Organisation { get; set; } = null!;

        public static implicit operator Member(CreateEmployerMemberCommand command) => new()
        {
            Id = command.Id,
            UserType = MembershipUserType.Employer,
            Joined = command.Joined,
            Information = command.Information,
            Status = MembershipStatus.Live,
            MemberRegions = Member.GenerateMemberRegions(command.Regions, command.Id),
            Employer = new Employer
            {
                MemberId = command.Id,
                AccountId = command.AccountId,
                UserRef = command.UserRef,
                Email = command.Email,
                Organisation = command.Organisation,
                LastUpdated = DateTime.Now,
                Name = command.Name
            }
        };
    }
}