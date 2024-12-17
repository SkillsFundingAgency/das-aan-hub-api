using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.MemberNotificationEventFormats.Queries.GetMemberNotificationEventFormats;

public class MemberNotificationEventFormatModel
{
    public long Id { get; set; }
    public Guid MemberId { get; set; }
    public string EventFormat { get; set; } = string.Empty;
    public int Ordering { get; set; }
    public bool ReceiveNotifications { get; set; }

    public static implicit operator MemberNotificationEventFormatModel(MemberNotificationEventFormat source) => new()
    {
        Id = source.Id,
        MemberId = source.MemberId,
        EventFormat = source.EventFormat,
        Ordering = source.Ordering,
        ReceiveNotifications = source.ReceiveNotifications
    };
}
