using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class AdminsWriteRepository : IAdminsWriteRepository
    {
        private readonly AanDataContext _aanDataContext;

        public AdminsWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public void Create(Admin admin) => _aanDataContext.Admins.Add(admin);

        public async Task<Admin?> GetPatchAdmin(string userName) => await _aanDataContext
            .Admins
            .Where(a => a.UserName == userName)
            .SingleOrDefaultAsync();
    }
}