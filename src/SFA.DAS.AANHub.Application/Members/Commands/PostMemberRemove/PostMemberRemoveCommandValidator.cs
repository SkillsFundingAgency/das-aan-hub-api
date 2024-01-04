using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators.AdminMemberId;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Members.Commands.PostMemberRemove;

public class PostMemberRemoveCommandValidator : AbstractValidator<PostMemberRemoveCommand>
{
    public const string PostMemberStatusCommandNotExpectedStatus = "The status must be 'Removed' or 'Deleted'";

    public PostMemberRemoveCommandValidator(IMembersReadRepository membersReadRepository)
    {
        Include(new MemberIdValidator(membersReadRepository));
        Include(new AdminMemberIdValidator(membersReadRepository));

        var conditions = new List<MembershipStatusType> { MembershipStatusType.Removed, MembershipStatusType.Deleted };

        RuleFor(c => c.Status)
            .Must(c => conditions.Contains(c))
            .WithMessage(PostMemberStatusCommandNotExpectedStatus);
    }
}