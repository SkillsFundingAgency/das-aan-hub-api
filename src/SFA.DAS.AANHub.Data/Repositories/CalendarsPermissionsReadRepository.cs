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
        private readonly IMembersPermissionsReadRepository _membersPermissionsReadRepository;

        public CalendarsPermissionsReadRepository(AanDataContext aanDataContext,
            IMembersPermissionsReadRepository membersPermissionsReadRepository)
        {
            _aanDataContext = aanDataContext;
            _membersPermissionsReadRepository = membersPermissionsReadRepository;
        }

        public async Task<List<CalendarPermission>> GetAllCalendarsPermissions() => await _aanDataContext.CalendarPermissions.AsNoTracking()
                .ToListAsync();

        public async Task<List<CalendarPermission>> GetAllCalendarsPermissionsForUser(Guid id)
        {
            var permissionIds = await _membersPermissionsReadRepository.GetAllMemberPermissionsForUser(id);

            return await _aanDataContext.CalendarPermissions
                    .Where(cp => permissionIds.Contains(cp.PermissionId)).AsNoTracking()
                    .ToListAsync();

        }
    }
}
