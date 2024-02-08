using FluentValidation;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Common;

public class CreateMemberCommandBaseValidator : AbstractValidator<CreateMemberCommandBase>
{
    private const string InvalidRegionErrorMessage = "Region value is invalid";
    public const string InvalidEmailFormatErrorMessage = "Email value is not in correct format";
    public const string ValueIsRequiredErrorMessage = "A valid value for {0} is required";
    public const string ExceededAllowableLengthErrorMessage = "Value for {0} cannot exceed character length of {1}";
    public const string EmailAlreadyExistsErrorMessage = "This email already exists";

    public CreateMemberCommandBaseValidator(IMembersReadRepository membersReadRepository, IRegionsReadRepository regionsReadRepository)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(string.Format(ValueIsRequiredErrorMessage, nameof(CreateMemberCommandBase.Email)))
            .MaximumLength(256)
            .WithMessage(string.Format(ExceededAllowableLengthErrorMessage, nameof(CreateMemberCommandBase.Email), 256))
            .Matches(Constants.RegularExpressions.EmailRegex)
            .WithMessage(InvalidEmailFormatErrorMessage)
            .MustAsync(async (email, cancellation) =>
            {
                var member = await membersReadRepository.GetMemberByEmail(email);
                return member == null;
            })
            .WithMessage(EmailAlreadyExistsErrorMessage);

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
            .MustAsync(async (regionId, cancellation) =>
            {
                if (regionId == null) return true;
                var regions = await regionsReadRepository.GetAllRegions(cancellation);
                return regions.Exists(x => x.Id == regionId);
            })
            .WithMessage(InvalidRegionErrorMessage);
        RuleFor(x => x.OrganisationName)
            .NotEmpty()
            .WithMessage(string.Format(ValueIsRequiredErrorMessage, nameof(CreateMemberCommandBase.OrganisationName)))
            .MaximumLength(250)
            .WithMessage(string.Format(ExceededAllowableLengthErrorMessage, nameof(CreateMemberCommandBase.OrganisationName), 250));
    }
}
