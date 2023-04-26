using FluentValidation;

namespace SFA.DAS.AANHub.Application.Common;

public class ProfileValueValidator : AbstractValidator<ProfileValue>
{
    public const string ProfileIdIsMissingErrorMessage = "Profile Id should have a valid value";
    public const string ProfileValueMustNotBeEmpty = "Profile with Id {0} must have value";
    public ProfileValueValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage(ProfileIdIsMissingErrorMessage);
        RuleFor(p => p.Value).NotEmpty().WithMessage(p => string.Format(ProfileValueMustNotBeEmpty, p.Id));
    }
}
