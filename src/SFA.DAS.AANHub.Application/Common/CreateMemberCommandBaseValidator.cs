using FluentValidation;

namespace SFA.DAS.AANHub.Application.Common;

public class CreateMemberCommandBaseValidator : AbstractValidator<CreateMemberCommandBase>
{
    private const string InvalidRegionErrorMessage = "Region value must be in between 1 and 9";
    public const string InvalidEmailFormatErrorMessage = "Email value is not in correct format";
    public const string ValueIsRequiredErrorMessage = "A valid value for {0} is required";
    public const string ExceededAllowableLengthErrorMessage = "Value for {0} cannot exceed character length of {1}";

    public CreateMemberCommandBaseValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(string.Format(ValueIsRequiredErrorMessage, nameof(CreateMemberCommandBase.Email)))
            .MaximumLength(256)
            .WithMessage(string.Format(ExceededAllowableLengthErrorMessage, nameof(CreateMemberCommandBase.Email), 256))
            .Matches(Constants.RegularExpressions.EmailRegex)
            .WithMessage(InvalidEmailFormatErrorMessage);
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .WithMessage(string.Format(ValueIsRequiredErrorMessage, nameof(CreateMemberCommandBase.FirstName)))
            .MaximumLength(200)
            .WithMessage(string.Format(ExceededAllowableLengthErrorMessage, nameof(CreateMemberCommandBase.FirstName), 200));
        RuleFor(c => c.LastName)
            .NotEmpty()
            .WithMessage(string.Format(ValueIsRequiredErrorMessage, nameof(CreateMemberCommandBase.LastName)))
            .MaximumLength(200)
            .WithMessage(string.Format(ExceededAllowableLengthErrorMessage, nameof(CreateMemberCommandBase.LastName), 200));
        RuleFor(x => x.JoinedDate)
            .NotEmpty()
            .WithMessage(string.Format(ValueIsRequiredErrorMessage, nameof(CreateMemberCommandBase.JoinedDate)))
            .LessThan(DateTime.Today.AddDays(1).Date);
        RuleFor(x => x.RegionId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(ValueIsRequiredErrorMessage, nameof(CreateMemberCommandBase.RegionId)))
            .InclusiveBetween(1, 9)
            .WithMessage(InvalidRegionErrorMessage);
        RuleFor(x => x.OrganisationName)
            .NotEmpty()
            .WithMessage(string.Format(ValueIsRequiredErrorMessage, nameof(CreateMemberCommandBase.OrganisationName)))
            .MaximumLength(250)
            .WithMessage(string.Format(ExceededAllowableLengthErrorMessage, nameof(CreateMemberCommandBase.OrganisationName), 250));
    }
}
