using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Admins.Commands
{
    public class CreateAdminMemberCommand : CreateMemberCommandBase, IRequest<ValidatableResponse<CreateAdminMemberCommandResponse>>, IRequestedByMemberId
    {
        public string? UserName { get; set; }
        public Guid? RequestedByMemberId { get; set; }

        public static implicit operator Member(CreateAdminMemberCommand command) => new()
        {
            Id = command.Id,
            UserType = MembershipUserType.Admin,
            Joined = command.Joined,
            Information = command.Information,
            Created = DateTime.Now,
            Updated = DateTime.Now,
            ReviewStatus = MembershipReviewStatus.New,
            Deleted = null,
            Status = MembershipStatus.Live,
            MemberRegions = Member.GenerateMemberRegions(command.Regions, command.Id),
            Admin = new Admin
            {
                MemberId = command.Id,
                Email = command.Email,
                UserName = command.UserName,
                LastUpdated = DateTime.Now,
                IsActive = true
            }
        };
    }
}
