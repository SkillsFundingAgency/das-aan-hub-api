namespace SFA.DAS.AANHub.Application.Extensions;
public static class DateAndTimeExtensions
{
    private static readonly TimeZoneInfo LocalTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
    public static DateTime UtcToLocalTime(this DateTime date) => TimeZoneInfo.ConvertTimeFromUtc(date, LocalTimeZone);
}
