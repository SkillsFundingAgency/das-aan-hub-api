using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Common;
public static class NotificationHelper
{
    public static Notification CreateNotification(Guid memberId, string templateName, string tokens, bool isSystem) => new Notification()
    {
        MemberId = memberId,
        TemplateName = templateName,
        Tokens = tokens,
        CreatedDate = DateTime.UtcNow,
        IsSystem = isSystem
    };
}
