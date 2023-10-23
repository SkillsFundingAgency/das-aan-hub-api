namespace SFA.DAS.AANHub.Application;

public static class Constants
{
    public static class RegularExpressions
    {
        public const string UrlRegex = @"^(?:[Hh][Tt][Tt][Pp]([Ss])?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
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
