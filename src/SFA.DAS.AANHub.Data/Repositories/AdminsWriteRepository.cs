using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class AdminsWriteRepository : IAdminsWriteRepository
    {
        private readonly AanDataContext _aanDataContext;

        public AdminsWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public async Task Create(Admin admin)
        {
            _aanDataContext.Admins.Add(admin);
            await _aanDataContext.SaveChangesAsync();
        }
    }
}
