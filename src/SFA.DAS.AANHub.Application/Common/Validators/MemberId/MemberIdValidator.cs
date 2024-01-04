using FluentValidation;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Common.Validators.MemberId;

public class MemberIdValidator : AbstractValidator<IMemberId>
{
    public const string MemberIdEmptyErrorMessage = "MemberId is empty";
    public const string MemberIdMustBeLive = "MemberId must be active";
    public const string MemberIdMustBeApprenticeOrEmployer = "MemberId must be apprentice or employer";


    public MemberIdValidator(IMembersReadRepository membersReadRepository)
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
                    member != null &&
                    member!.Status == MembershipStatusType.Live.ToString();
            })
            .WithMessage(MemberIdMustBeLive)
            .Must((_) =>
                member is { UserType: UserType.Apprentice or UserType.Employer })
            .WithMessage(MemberIdMustBeApprenticeOrEmployer);
    }
}
