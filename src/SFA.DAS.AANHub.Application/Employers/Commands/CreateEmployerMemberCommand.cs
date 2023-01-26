using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Employers.Commands
{
    public class CreateEmployerMemberCommand : CreateMemberCommandBase, IRequest<ValidatedResponse<CreateEmployerMemberCommandResponse>>, IRequestedByMemberId
    {
        public long AccountId { get; init; }
        public long UserId { get; init; }
        public string? Organisation { get; init; }
        public Guid? RequestedByMemberId { get; set; }

        public static implicit operator Member(CreateEmployerMemberCommand command) => new()
        {
            Id = command.Id,
            UserType = MembershipUserType.Employer,
            Joined = command.Joined,
            Information = command.Information,
            ReviewStatus = MembershipReviewStatus.New,
            Status = MembershipStatus.Live,
            MemberRegions = Member.GenerateMemberRegions(command.Regions, command.Id),
            Employer = new Employer
            {
                MemberId = command.Id,
                AccountId = command.AccountId,
                UserId = command.UserId,
                Email = command.Email,
                Organisation = command.Organisation,
                LastUpdated = DateTime.Now,
                Name = command.Name,
                IsActive = true
            }
        };
    }
}