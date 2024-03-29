﻿using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Common;
public static class NotificationHelper
{
    public static Notification CreateNotification(Guid notificationId, Guid memberId, string templateName, string tokens, Guid createdBy, bool isSystem, string? referenceId) => new Notification()
    {
        Id = notificationId,
        MemberId = memberId,
        TemplateName = templateName,
        Tokens = tokens,
        CreatedDate = DateTime.UtcNow,
        CreatedBy = createdBy,
        IsSystem = isSystem,
        ReferenceId = referenceId
    };
}
