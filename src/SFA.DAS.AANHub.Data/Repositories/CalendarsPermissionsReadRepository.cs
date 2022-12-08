using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class CalendarsPermissionsReadRepository : ICalendarsPermissionsReadRepository
    {
        private readonly AanDataContext _aanDataContext;

        public CalendarsPermissionsReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public async Task<List<CalendarPermission>> GetAllCalendarsPermissions() => await _aanDataContext.CalendarPermissions.AsNoTracking()
                .ToListAsync();

        public async Task<List<CalendarPermission>> GetAllCalendarsPermissionsByPermissionIds(List<long> permissionIds) => await _aanDataContext.CalendarPermissions
                    .Where(cp => permissionIds.Contains(cp.PermissionId)).AsNoTracking()
                    .ToListAsync();
    }
}
