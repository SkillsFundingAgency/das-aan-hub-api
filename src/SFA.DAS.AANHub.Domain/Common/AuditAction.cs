using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Domain.Common;

[ExcludeFromCodeCoverage]
public static class AuditAction
{
    public const string Post = nameof(Post);
    public const string Create = nameof(Create);
    public const string Put = nameof(Put);
    public const string Cancelled = nameof(Cancelled);
    public const string PatchMember = nameof(PatchMember);
    public const string Withdrawn = nameof(Withdrawn);
}