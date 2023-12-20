using FluentValidation;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Common.Validators.AdminMemberId;
public class AdminMemberIdValidator : AbstractValidator<IAdminMemberId>
{
    public const string RequestedByMemberIdMustNotBeEmpty = "requestedByMemberId must have a value";
    public const string RequestedByMemberIdMustBeLive = "requestedByMemberId must be active";
    public const string RequestedByMemberIdMustBeAdmin = "requestedByMemberId must be an admin member or regional chair";

    public AdminMemberIdValidator(IMembersReadRepository membersReadRepository)
    {
        Member? member = null;
        RuleFor(c => c.AdminMemberId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(RequestedByMemberIdMustNotBeEmpty)
            .MustAsync(async (memberId, cancellationToken) =>
            {
                member = await membersReadRepository.GetMember(memberId);
                return
                    member != null &&
                    member!.Status == MembershipStatusType.Live.ToString();
            })
            .WithMessage(RequestedByMemberIdMustBeLive)
            .Must((_) => member != null && (member.UserType == UserType.Admin || member.IsRegionalChair.GetValueOrDefault()))
            .WithMessage(RequestedByMemberIdMustBeAdmin);
    }
}
