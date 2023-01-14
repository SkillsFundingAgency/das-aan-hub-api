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

        public static class MembershipUserType
        {
            public const string Apprentice = nameof(Apprentice);
            public const string Employer = nameof(Employer);
            public const string Partner = nameof(Partner);
            public const string Admin = nameof(Admin);
        }

        public static class MembershipReviewStatus
        {
            public const string New = nameof(New);
            public const string InProgress = nameof(InProgress);
            public const string Archived = nameof(Archived);
        }
    }
}
