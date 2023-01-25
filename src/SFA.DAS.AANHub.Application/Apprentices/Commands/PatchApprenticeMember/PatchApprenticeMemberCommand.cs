using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands.PatchApprenticeMember
{
    public class PatchApprenticeMemberCommand : IRequest<ValidatedResponse<PatchApprenticeMemberCommandResponse>>, IRequestedByMemberId
    {
        public const string EmailIdentifier = "Email";
        public const string NameIdentifier = "Name";

        public Guid RequestedByMemberId { get; set; }
        public long ApprenticeId { get; set; }
        public JsonPatchDocument<Apprentice> Patchdoc { get; set; } = null!;

        public string? Email =>
            Patchdoc.Operations.FirstOrDefault(operation =>
                operation.path == EmailIdentifier && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase))?.value.ToString();

        public string? Name =>
            Patchdoc.Operations.FirstOrDefault(operation =>
                operation.path == NameIdentifier && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase))?.value.ToString();

        public bool IsPresentEmail =>
            Patchdoc.Operations.Any(operation =>
                operation.path == EmailIdentifier && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase));

        public bool IsPresentName =>
            Patchdoc.Operations.Any(operation =>
                operation.path == NameIdentifier && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase));

    }
}
