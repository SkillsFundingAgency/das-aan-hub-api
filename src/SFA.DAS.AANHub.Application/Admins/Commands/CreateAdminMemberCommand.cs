using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Admins.Commands
{
    public class CreateAdminMemberCommand : CreateMemberCommandBase, IRequest<ValidatedResponse<CreateAdminMemberCommandResponse>>, IRequestedByMemberId
    {
        public string UserName { get; set; } = null!;
        public Guid? RequestedByMemberId { get; set; }

        public static implicit operator Member(CreateAdminMemberCommand command) => new()
        {
            Id = command.Id,
            UserType = MembershipUserType.Admin,
            Joined = command.Joined,
            Information = command.Information,
            ReviewStatus = MembershipReviewStatus.New,
            Status = MembershipStatus.Live,
            MemberRegions = Member.GenerateMemberRegions(command.Regions, command.Id),
            Admin = new Admin
            {
                MemberId = command.Id,
                Email = command.Email,
                Name = command.Name,
                UserName = command.UserName,
                LastUpdated = DateTime.Now
            }
        };
    }
}