using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Enums;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands
{
    public class CreateApprenticeMemberCommand : CreateMemberCommandBase, IRequest<CreateApprenticeMemberCommandResponse>, IRequestedByMemberId
    {
        public Guid? RequestedByMemberId { get; set; }
        public long ApprenticeId { get; set; }

        public static implicit operator Member(CreateApprenticeMemberCommand command) => new Member()
        {
            Id = command.Id,
            UserType = MembershipUserType.Apprentice,
            Joined = command.Joined,
            Information = command.Information,
            Created = DateTime.Now,
            Updated = DateTime.Now,
            ReviewStatus = MembershipReviewStatus.New,
            Deleted = null,
            Status = MembershipStatus.Live,
            Apprentice = new Apprentice
            {
                ApprenticeId = command.ApprenticeId,
                MemberId = command.Id,
                Email = command.Email,
                Name = command.Name,
                LastUpdated = DateTime.Now,
                IsActive = true
            }
        };
    }
}
