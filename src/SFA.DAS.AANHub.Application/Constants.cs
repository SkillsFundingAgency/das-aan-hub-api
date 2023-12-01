using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Application;

public static class Constants
{

    [ExcludeFromCodeCoverage]
    public static class RegularExpressions
    {
        public const string UrlRegex = @"(?i)^https:\/\/[(www\.)?a-zA-Z0-9@:%._\+~#=-]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";
        public const string EmailRegex = @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z0-9_](-?[a-zA-Z0-9_])*(\.[a-zA-Z0-9](-?[a-zA-Z0-9])*)+$";
        public const string PostcodeRegex = @"^[a-zA-z]{1,2}\d[a-zA-z\d]?\s*\d[a-zA-Z]{2}$";
        public const string ExcludedCharactersRegex = @"^[^@#$^=+\\\/<>%]*$";
    }

    public static class MembershipStatus
    {
        public const string Withdrawn = "withdrawn";
        public const string Deleted = "deleted";
    }

    public static class NotificationTemplateNames
    {
        public const string AANAdminEventUpdate = nameof(AANAdminEventUpdate);
        public const string AANAdminEventCancel = nameof(AANAdminEventCancel);
    }
}
