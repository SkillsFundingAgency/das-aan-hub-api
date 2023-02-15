using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace SFA.DAS.AANHub.Application.Common.Commands
{
    public abstract class PatchMemberCommandBase<T> where T : class
    {
        public DateTime LastUpdated { get; } = DateTime.Now;

        public JsonPatchDocument<T> PatchDoc { get; set; } = null!;

        protected static string? GetReplacementValue(JsonPatchDocument<T> patchDoc, string propertyIdentifier) => patchDoc.Operations
            .FirstOrDefault(operation =>
                operation.path == propertyIdentifier && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase))
            ?.value
            .ToString()?.Trim();

        protected static bool HasValue(JsonPatchDocument<T> patchDoc, string propertyName) => patchDoc.Operations.Any(operation =>
            operation.path == propertyName && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase));
    }
}