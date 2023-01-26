using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Partners
{
    public class CreatePartnerMemberCommand : CreateMemberCommandBase, IRequest<ValidatableResponse<CreatePartnerMemberCommandResponse>>, IRequestedByMemberId
    {
        public string UserName { get; set; } = null!;
        public string Organisation { get; set; } = null!;
        public Guid? RequestedByMemberId { get; set; }

        public static implicit operator Member(CreatePartnerMemberCommand command) => new()
        {
            Id = command.Id,
            UserType = MembershipUserType.Partner,
            Joined = command.Joined,
            Information = command.Information,
            Created = DateTime.Now,
            Updated = DateTime.Now,
            ReviewStatus = MembershipReviewStatus.New,
            Deleted = null,
            Status = MembershipStatus.Live,
            MemberRegions = Member.GenerateMemberRegions(command.Regions, command.Id),
            Partner = new Partner
            {
                MemberId = command.Id,
                Email = command.Email,
                UserName = command.UserName,
                Organisation = command.Organisation,
                LastUpdated = DateTime.Now,
                IsActive = true
            }
        };
    }
}