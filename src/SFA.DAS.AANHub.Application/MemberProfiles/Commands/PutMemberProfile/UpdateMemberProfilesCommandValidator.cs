using FluentValidation;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.MemberProfiles.Commands.PutMemberProfile;
public class UpdateMemberProfilesCommandValidator : AbstractValidator<UpdateMemberProfilesCommand>
{
    public const string MemberIdNotRecognisedErrorMessage = "Member record not found";
    public const string ValueIsRequiredErrorMessage = "A valid value for {0} is required";

    public UpdateMemberProfilesCommandValidator(IMembersReadRepository membersReadRepository)
    {
        RuleFor(x => x.MemberId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(ValueIsRequiredErrorMessage, nameof(UpdateMemberProfilesCommand.MemberId)))
            .MustAsync(async (memberId, cancellation) =>
            {
                var member = await membersReadRepository.GetMember(memberId);
                return member != null;
            })
            .WithMessage(MemberIdNotRecognisedErrorMessage);
    }
}
