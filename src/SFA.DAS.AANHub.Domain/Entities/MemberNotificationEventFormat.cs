namespace SFA.DAS.AANHub.Domain.Entities;
public class MemberNotificationEventFormat
{
    public long Id { get; set; }
    public Guid MemberId { get; set; }
    public string EventFormat { get; set; } = string.Empty;
    public int Ordering { get; set; }
    public bool ReceiveNotifications { get; set; }
}
