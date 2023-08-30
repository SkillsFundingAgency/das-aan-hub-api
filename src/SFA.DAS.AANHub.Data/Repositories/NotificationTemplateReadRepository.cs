using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class NotificationTemplateReadRepository : INotificationTemplateReadRepository
{
    private readonly IAanDataContext _aanDataContext;

    public NotificationTemplateReadRepository(IAanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public async Task<NotificationTemplate?> Get(long notificationTemplateId, CancellationToken cancellationToken) => await _aanDataContext
        .NotificationTemplates
        .AsNoTracking()
        .Where(nt => nt.Id == notificationTemplateId)
        .SingleOrDefaultAsync(cancellationToken);
}