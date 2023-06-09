using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class CalendarsReadRepository : ICalendarsReadRepository
{
    private readonly AanDataContext _aanDataContext;

    public CalendarsReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public async Task<List<Calendar>> GetAllCalendars(CancellationToken cancellationToken) =>
        await _aanDataContext
            .Calendars
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}