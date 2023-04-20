using Microsoft.AspNetCore.JsonPatch;

namespace SFA.DAS.AANHub.Application.Extensions;

public static class JsonPatchDocumentExtensions
{
    public static string? GetReplacementValue<T>(this JsonPatchDocument<T> patchDoc, string propertyIdentifier) where T : class
        => patchDoc.Operations.FirstOrDefault(operation => (FormattedPath(operation.path)) == propertyIdentifier)?.value?.ToString()?.Trim();

    public static int? GetReplacementValueAsInt<T>(this JsonPatchDocument<T> patchDoc, string propertyIdentifier) where T : class
    {
        return int.TryParse(GetReplacementValue(patchDoc, propertyIdentifier), out var result) ? result : null;
    }

    public static bool HasValue<T>(this JsonPatchDocument<T> patchDoc, string propertyName) where T : class
        => patchDoc.PatchOperationsFieldListInLowerCase().Any(field => field == propertyName.ToLower());

    public static IEnumerable<string> PatchOperationsFieldListInLowerCase<T>(this JsonPatchDocument<T> patchDocument) where T : class
        => patchDocument.Operations.Select(o => FormattedPath(o.path));

    private static string FormattedPath(string path) => path.Trim().Replace("/", string.Empty).ToLower();
}
