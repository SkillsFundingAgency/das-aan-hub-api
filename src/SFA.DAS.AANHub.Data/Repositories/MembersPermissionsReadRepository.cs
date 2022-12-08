using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class MembersPermissionsReadRepository : IMembersPermissionsReadRepository
    {
        private readonly AanDataContext _aanDataContext;

        public MembersPermissionsReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public async Task<List<long>> GetAllMemberPermissionsForUser(Guid id) => await _aanDataContext
                .MemberPermissions.AsNoTracking().Where(m => m.MemberId == id && m.IsActive)
                .Select(m => m.PermissionId)
                .Distinct()
                .ToListAsync();
    }
}
