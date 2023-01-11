using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Domain.Common
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        public static class MembershipStatus
        {
            public const string Pending = nameof(Pending);
            public const string Cancelled = nameof(Cancelled);
            public const string Live = nameof(Live);
            public const string Withdrawn = nameof(Withdrawn);
            public const string Deleted = nameof(Deleted);
        }
    }
}
