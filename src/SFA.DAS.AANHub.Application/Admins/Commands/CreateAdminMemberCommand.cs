using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Enums;

namespace SFA.DAS.AANHub.Application.Admins.Commands
{
    public class CreateAdminMemberCommand : CreateMemberCommandBase, IRequest<CreateAdminMemberCommandResponse>
    {
        public string? UserName { get; set; }

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
