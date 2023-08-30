namespace SFA.DAS.AANHub.Api.Models;

public class CreateNotificationModel
{
    public Guid MemberId { get; set; }
    public int NotificationTemplateId { get; set; }
}
