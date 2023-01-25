using System.Collections.ObjectModel;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch.Operations;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands.PatchApprenticeMember
{
    public class PatchApprenticeMemberCommandValidator : AbstractValidator<PatchApprenticeMemberCommand>
    {
        public const string NoPatchOperationsPresentErrorMessage = "There are no patch operations in this call";
        public const string PatchOperationsLimitExceededErrorMessage = "The number of patch operations in this call exceed the expected count";
        public const string PatchOperationContainsUnavailableFieldErrorMessage = "This patch operation contains an unexpected field";
        public const string PatchOperationContainsUnavailableOperationErrorMessage = "This patch operation contains an unexpected operation";

        public const string EmailIdentifier = "Email";
        public const string NameIdentifier = "Name";

        public static readonly IList<string> PatchFields = new ReadOnlyCollection<string>(
                new List<string>
                {
                    "Email",
                    "Name"
                });

        public PatchApprenticeMemberCommandValidator(IMembersReadRepository membersReadRepository)
        {
            Include(new RequestedByMemberIdValidator(membersReadRepository));

            RuleFor(c => c.Patchdoc.Operations.Count).GreaterThan(0).WithMessage(NoPatchOperationsPresentErrorMessage);

            RuleFor(c => c.Patchdoc.Operations.Count(operation => !operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase)))
                .Equal(0)
                .WithMessage(PatchOperationContainsUnavailableOperationErrorMessage);

            foreach (var patchfield in PatchFields)
            {
                RuleFor(c => c.Patchdoc.Operations.Count(operation => operation.path == patchfield && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase)))
                .LessThan(2)
                .WithMessage(PatchOperationsLimitExceededErrorMessage);
            }

            RuleFor(c => c.Patchdoc.Operations.Count(operation => !PatchFields.Contains(operation.path)))
                .Equal(0)
                .WithMessage(PatchOperationContainsUnavailableFieldErrorMessage);

            RuleFor(x => x.ApprenticeId)
                .NotNull()
                .NotEmpty();

            When(x => x.IsPresentEmail, () =>
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .MaximumLength(256)
                    .Matches(Constants.RegularExpressions.EmailRegex);
            });

            When(x => x.IsPresentName, () =>
            {
                RuleFor(c => c.Name)
                    .NotEmpty()
                    .MaximumLength(200);
            });
        }
    }
}