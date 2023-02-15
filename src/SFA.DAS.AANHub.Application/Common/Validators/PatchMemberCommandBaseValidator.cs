using FluentValidation;
using Microsoft.AspNetCore.JsonPatch.Operations;
using SFA.DAS.AANHub.Application.Common.Commands;

namespace SFA.DAS.AANHub.Application.Common.Validators
{
    public class PatchMemberCommandBaseValidator<T> : AbstractValidator<PatchMemberCommandBase<T>> where T : class
    {
        private const string NoPatchOperationsPresentErrorMessage = "There are no patch operations in this request";
        private const string FoundDuplicatePatchOperationsErrorMessage = "There are duplicate patch operations in this request";
        private const string PatchOperationContainsUnavailableFieldErrorMessage = "This patch operation contains an unexpected field";
        private const string PatchOperationContainsUnavailableOperationErrorMessage = "This patch operation contains an unexpected operation";

        public PatchMemberCommandBaseValidator(List<string> patchFields)
        {
            RuleFor(c => c.PatchDoc.Operations.Count).GreaterThan(0).WithMessage(NoPatchOperationsPresentErrorMessage);

            RuleFor(c => c.PatchDoc.Operations.Count(operation
                    => !operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase)))
                .Equal(0)
                .WithMessage(PatchOperationContainsUnavailableOperationErrorMessage);

            foreach (var patchField in patchFields)
                RuleFor(c => c.PatchDoc.Operations.Count(operation
                        => operation.path == patchField
                    ))
                    .LessThan(2)
                    .WithMessage(FoundDuplicatePatchOperationsErrorMessage);

            RuleFor(c => c.PatchDoc.Operations.Count(operation => !patchFields.Contains(operation.path)))
                .Equal(0)
                .WithMessage(PatchOperationContainsUnavailableFieldErrorMessage);
        }
    }
}