using FluentValidation;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Members.Commands.PostMemberReinstate;

public class PostMemberReinstateCommandValidator : AbstractValidator<PostMemberReinstateCommand>
{
    public const string MemberIdEmptyErrorMessage = "MemberId is empty";
    public const string MemberIdMustBeWithdrawnOrDeleted = "Member status must be Withdrawn or Deleted";
    public const string MemberIdMustBeApprenticeOrEmployer = "Member must be apprentice or employer";

    public PostMemberReinstateCommandValidator(IMembersReadRepository membersReadRepository)
    {

        Member? member = null;
        RuleFor(x => x.MemberId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(MemberIdEmptyErrorMessage)
            .MustAsync(async (memberId, cancellationToken) =>
            {
                member = await membersReadRepository.GetMember(memberId);
                return
                    member is { UserType: UserType.Apprentice or UserType.Employer };
            })
            .WithMessage(MemberIdMustBeApprenticeOrEmployer)
            .Must((_) =>
                  (member!.Status == MembershipStatusType.Withdrawn.ToString()
                                 || member.Status == MembershipStatusType.Deleted.ToString()))
            .WithMessage(MemberIdMustBeWithdrawnOrDeleted);
    }
}