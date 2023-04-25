using Microsoft.AspNetCore.JsonPatch;

namespace SFA.DAS.AANHub.Application.Extensions;

public static class JsonPatchDocumentExtensions
{
    public static string? GetReplacementValue<T>(this JsonPatchDocument<T> patchDoc, string propertyIdentifier) where T : class
        => patchDoc.Operations.FirstOrDefault(operation => (GetNormalisedKey(operation.path)) == GetNormalisedKey(propertyIdentifier))?.value?.ToString()?.Trim();

    public static int? GetReplacementValueAsInt<T>(this JsonPatchDocument<T> patchDoc, string propertyIdentifier) where T : class
    {
        return int.TryParse(GetReplacementValue(patchDoc, propertyIdentifier), out var result) ? result : null;
    }

    public static bool HasValue<T>(this JsonPatchDocument<T> patchDoc, string propertyName) where T : class
        => patchDoc.PatchOperationsFieldList().Any(field => field == GetNormalisedKey(propertyName));

    public static IEnumerable<string> PatchOperationsFieldList<T>(this JsonPatchDocument<T> patchDocument) where T : class
        => patchDocument.Operations.Select(o => GetNormalisedKey(o.path));

    private static string GetNormalisedKey(string path) => path.Trim().Replace("/", string.Empty).ToLower();
}
