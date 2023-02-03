using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class EmployersReadRepository : IEmployersReadRepository
    {
        private readonly AanDataContext _aanDataContext;

        public EmployersReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public async Task<Employer?> GetEmployerByAccountIdAndUserId(long accountId, long userId) => await _aanDataContext
            .Employers
            .AsNoTracking().Where(m => m.AccountId == accountId && m.UserId == userId)
            .SingleOrDefaultAsync();
    }
}
