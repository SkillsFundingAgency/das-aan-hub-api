using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;
public interface INotificationTemplateReadRepository
{
    Task<NotificationTemplate?> Get(long notificationTemplateId, CancellationToken cancellationToken);
}
