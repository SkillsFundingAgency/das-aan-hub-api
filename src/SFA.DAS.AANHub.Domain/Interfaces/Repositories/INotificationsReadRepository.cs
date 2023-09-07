using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;
public interface INotificationsReadRepository
{
    Task<Notification?> GetNotificationById(Guid notificationId, CancellationToken cancellationToken);

}
