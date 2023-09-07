using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class NotificationsReadRepository : INotificationsReadRepository
{
    private readonly AanDataContext _aanDataContext;

    public NotificationsReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public async Task<Notification?> GetNotificationById(Guid notificationId, CancellationToken cancellationToken) => await _aanDataContext
        .Notifications
        .AsNoTracking()
        .Where(n => n.Id == notificationId)
        .SingleOrDefaultAsync(cancellationToken);
}
