using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class AdminsReadRepository : IAdminsReadRepository
    {
        private readonly AanDataContext _aanDataContext;

        public AdminsReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public async Task<Admin?> GetAdminByUserName(string userName) => await _aanDataContext
            .Admins
            .AsNoTracking().Where(a => a.UserName == userName)
            .SingleOrDefaultAsync();
    }
}
