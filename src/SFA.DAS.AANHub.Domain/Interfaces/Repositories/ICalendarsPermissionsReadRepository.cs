using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface ICalendarsPermissionsReadRepository
    {
        Task<List<CalendarPermission>> GetAllCalendarsPermissions();
        Task<List<CalendarPermission>> GetAllCalendarsPermissionsByPermissionIds(List<long> permissionIds);
    }
}
