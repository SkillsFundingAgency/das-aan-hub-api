﻿namespace SFA.DAS.AANHub.Application;

public static class Constants
{
    public static class RegularExpressions
    {
        public const string UrlRegex = @"^[Hh][Tt][Tt][Pp][Ss]://(?:[Ww][Ww][Ww]\\.)?[a-zA-Z0-9-]+\\.[a-zA-Z]{2,}(/\\S*)?$";
        public const string EmailRegex = @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z0-9_](-?[a-zA-Z0-9_])*(\.[a-zA-Z0-9](-?[a-zA-Z0-9])*)+$";
        public const string PostcodeRegex = @"^[a-zA-z]{1,2}\d[a-zA-z\d]?\s*\d[a-zA-Z]{2}$";
        public const string ExcludedCharactersRegex = @"^[^@#$^=+\\\/<>%]*$";
    }

    public static class MembershipStatus
    {
        public const string Withdrawn = "withdrawn";
        public const string Deleted = "deleted";
    }
}
