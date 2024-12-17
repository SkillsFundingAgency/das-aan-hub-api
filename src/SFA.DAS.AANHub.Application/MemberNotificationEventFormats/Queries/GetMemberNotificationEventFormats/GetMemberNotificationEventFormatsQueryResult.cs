namespace SFA.DAS.AANHub.Application.MemberNotificationEventFormats.Queries.GetMemberNotificationEventFormats;

public class GetMemberNotificationEventFormatsQueryResult
{
    public IEnumerable<MemberNotificationEventFormatModel> MemberNotificationEventFormats { get; set; } = Enumerable.Empty<MemberNotificationEventFormatModel>();
}
