using FluentValidation;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Common.Validators.MemberId;
public class MemberIdValidator : AbstractValidator<IMemberId>
{
    public const string MemberIdEmptyErrorMessage = "MemberId is empty";
    public const string MemberIdNotFoundErrorMessage = "Could not find a valid active Member ID matching the request member ID";

    public MemberIdValidator(IMembersReadRepository membersReadRepository) => RuleFor(x => x.MemberId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(MemberIdEmptyErrorMessage)
            .MustAsync(async (memberId, cancellation) =>
            {
                var member = await membersReadRepository.GetMember(memberId);
                return member is { Status: MembershipStatus.Live, UserType: "Apprentice" or "Employer" };
            })
            .WithMessage(MemberIdNotFoundErrorMessage);
}
