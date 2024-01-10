using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Members.Commands.PostMemberLeaving;

public class PostMemberLeavingCommandValidator : AbstractValidator<PostMemberLeavingCommand>
{
    public PostMemberLeavingCommandValidator(IMembersReadRepository membersReadRepository)
    {
        Include(new MemberIdValidator(membersReadRepository));
    }
}