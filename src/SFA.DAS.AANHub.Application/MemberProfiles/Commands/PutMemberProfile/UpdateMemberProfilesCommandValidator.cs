using FluentValidation;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.MemberProfiles.Commands.PutMemberProfile;
public class UpdateMemberProfilesCommandValidator : AbstractValidator<UpdateMemberProfilesCommand>
{
    public const string MemberIdNotRecognisedErrorMessage = "Member record not found";
    public const string ValueIsRequiredErrorMessage = "A valid value for {0} is required";
    public const string MemberStatusIsNotLIVE = "Profile changes are restricted to AAN Members that have the status as Live";

    public UpdateMemberProfilesCommandValidator(IMembersReadRepository membersReadRepository)
    {
        Member? member = null;
        RuleFor(x => x.MemberId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(ValueIsRequiredErrorMessage, nameof(UpdateMemberProfilesCommand.MemberId)))
            .MustAsync(async (memberId, cancellation) =>
            {
                member = await membersReadRepository.GetMember(memberId);
                return member != null;
            })
            .WithMessage(MemberIdNotRecognisedErrorMessage)
            .Must((_) => member!.Status == MembershipStatus.Live)
            .WithMessage(MemberStatusIsNotLIVE);
    }
}
