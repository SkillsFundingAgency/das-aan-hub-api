using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Enums;

namespace SFA.DAS.AANHub.Application.Employers.Commands
{
    public class CreateEmployerMemberCommand : CreateMemberCommandBase, IRequest<CreateEmployerMemberCommandResponse>
    {
        public long AccountId { get; set; }
        public long UserId { get; set; }
        public string? Organisation { get; set; }
        public Guid? RequestedByUserId { get; set; }

        public static implicit operator Member(CreateEmployerMemberCommand command) => new()
        {
            Id = command.Id,
            UserType = MembershipUserType.Employer,
            Joined = command.Joined,
            Information = command.Information,
            Created = DateTime.Now,
            Updated = DateTime.Now,
            ReviewStatus = MembershipReviewStatus.New,
            Deleted = null,
            Status = MembershipStatus.Live,
            Employer = new Employer
            {
                MemberId = command.Id,
                AccountId = command.AccountId,
                Email = command.Email,
                Organisation = command.Organisation,
                LastUpdated = DateTime.Now,
                Name = command.Name,
                IsActive = true
            }
        };

    }
}
