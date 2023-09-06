using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;
public interface INotificationsWriteRepository
{
    void Create(Notification notification);
}
