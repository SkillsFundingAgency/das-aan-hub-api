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

        public async Task<Employer?> GetEmployerByUserRef(Guid userRef) => await _aanDataContext
            .Employers
            .AsNoTracking()
            .Where(m => m.UserRef == userRef).Include(x => x.Member)
            .SingleOrDefaultAsync();

        public async Task<Employer?> GetEmployerByMemberId(Guid memberId, CancellationToken cancellationToken) => await _aanDataContext
            .Employers
            .AsNoTracking()
            .Where(m => m.MemberId == memberId).Include(x => x.Member)
            .SingleOrDefaultAsync(cancellationToken);
    }
}