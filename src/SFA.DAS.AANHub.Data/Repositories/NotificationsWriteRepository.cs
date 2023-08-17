using System.Diagnostics.CodeAnalysis;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class NotificationsWriteRepository : INotificationsWriteRepository
{
    private readonly AanDataContext _aanDataContext;

    public NotificationsWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public void Create(Notification notification) => _aanDataContext.Notifications.Add(notification);
}
