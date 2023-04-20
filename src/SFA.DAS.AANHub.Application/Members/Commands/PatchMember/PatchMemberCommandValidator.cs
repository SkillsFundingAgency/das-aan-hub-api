﻿using FluentValidation;
using Microsoft.AspNetCore.JsonPatch.Operations;
using SFA.DAS.AANHub.Application.Extensions;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Members.Commands.PatchMember;

public class PatchMemberCommandValidator : AbstractValidator<PatchMemberCommand>
{
    public const string MemberIdNotRecognisedErrorMessage = "Member record not found";
    public const string NoPatchOperationsPresentErrorMessage = "There are no patch operations in this request";
    public static readonly string TooManyPatchOperationsPresentErrorMessage = $"There are too many patch operations in this request, a maximum of {MemberPatchFields.PatchFields.Length} operations are allowed";
    public const string FoundDuplicatePatchOperationsErrorMessage = "There are duplicate patch operations in this request";
    public readonly static string PatchOperationContainsUnavailableFieldErrorMessage = $"This patch request contains an unexpected path, allowable paths are '{string.Join(',', MemberPatchFields.PatchFields)}'";
    public const string PatchOperationContainsUnavailableOperationErrorMessage = "This patch operation contains an unexpected operation";
    public const string InvalidEmailFormatErrorMessage = "Email value is not in correct format";
    public const string ValueIsRequiredErrorMessage = "A valid value for {0} is required";
    public const string ExceededAllowableLengthErrorMessage = "Value for {0} cannot exceed character length of {1}";

    public static readonly string[] ValidStatuses = new[] { "Deleted", "Withdrawn" };

    public PatchMemberCommandValidator(IMembersReadRepository membersReadRepository)
    {
        RuleFor(x => x.MemberId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(ValueIsRequiredErrorMessage, nameof(PatchMemberCommand.MemberId)))
            .MustAsync(async (memberId, cancellation) =>
            {
                var member = await membersReadRepository.GetMember(memberId);
                return member != null;
            })
            .WithMessage(MemberIdNotRecognisedErrorMessage);

        RuleFor(c => c.PatchDoc)
            .Must(p => p.Operations.Count > 0)
            .WithMessage(NoPatchOperationsPresentErrorMessage)
            .Must(p => p.Operations.Count <= MemberPatchFields.PatchFields.Length)
            .WithMessage(TooManyPatchOperationsPresentErrorMessage);

        RuleFor(c => c.PatchDoc)
            .Must(d => d.Operations.All(operation => operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase)))
            .WithMessage(PatchOperationContainsUnavailableOperationErrorMessage);

        RuleFor(c => c.PatchDoc)
            .Must(c => c.PatchOperationsFieldListInLowerCase().GroupBy(s => s).Any(g => g.Count() < 2))
            .WithMessage(FoundDuplicatePatchOperationsErrorMessage);

        RuleFor(c => c.PatchDoc)
            .Must(p => p.PatchOperationsFieldListInLowerCase().All(f => MemberPatchFields.PatchFields.Contains(f)))
            .WithMessage(PatchOperationContainsUnavailableFieldErrorMessage);

        When(x => x.HasEmail,
            () =>
            {
                RuleFor(x => x.Email)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage(string.Format(ValueIsRequiredErrorMessage, MemberPatchFields.Email))
                    .MaximumLength(256)
                    .WithMessage(string.Format(ExceededAllowableLengthErrorMessage, MemberPatchFields.Email, 256))
                    .Matches(Constants.RegularExpressions.EmailRegex)
                    .WithMessage(InvalidEmailFormatErrorMessage);
            });

        When(x => x.HasFirstName,
            () =>
            {
                RuleFor(c => c.FirstName)
                    .NotEmpty()
                    .WithMessage(string.Format(ValueIsRequiredErrorMessage, MemberPatchFields.FirstName))
                    .MaximumLength(200)
                    .WithMessage(string.Format(ExceededAllowableLengthErrorMessage, MemberPatchFields.FirstName, 200));
            });

        When(x => x.HasLastName,
            () =>
            {
                RuleFor(c => c.LastName)
                    .NotEmpty()
                    .WithMessage(string.Format(ValueIsRequiredErrorMessage, MemberPatchFields.LastName))
                    .MaximumLength(200)
                    .WithMessage(string.Format(ExceededAllowableLengthErrorMessage, MemberPatchFields.LastName, 200));
            });

        When(x => x.HasOrganisationName,
            () =>
            {
                RuleFor(c => c.OrganisationName)
                    .NotEmpty()
                    .WithMessage(string.Format(ValueIsRequiredErrorMessage, MemberPatchFields.OrganisationName))
                    .MaximumLength(250)
                    .WithMessage(string.Format(ExceededAllowableLengthErrorMessage, MemberPatchFields.OrganisationName, 250));
            });

        When(x => x.HasRegionId,
            () =>
            {
                RuleFor(c => c.RegionId)
                    .InclusiveBetween(1, 9);
            });

        When(x => x.HasStatus,
            () =>
            {
                RuleFor(c => c.Status)
                    .Must(s => ValidStatuses.Contains(s));
            });
    }
}
