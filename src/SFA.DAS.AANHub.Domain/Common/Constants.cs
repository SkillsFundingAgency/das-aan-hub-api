using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Domain.Common;

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
        public static readonly string Apprentice = UserType.Apprentice.ToString();
        public static readonly string Employer = UserType.Employer.ToString();
        public static readonly string Partner = UserType.Partner.ToString();
        public static readonly string Admin = UserType.Admin.ToString();
    }

    public static class CalendarEvents
    {
        public static readonly int PageSize = 5;
    }

    public static class ProfileDataId
    {
        public const int Longitude = 36;
        public const int Latitude = 37;
    }

    public static class Members
    {
        public static readonly int PageSize = 30;
    }
}
