using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class ApprenticesReadRepository : IApprenticesReadRepository
    {
        private readonly AanDataContext _aanDataContext;

        public ApprenticesReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public async Task<Apprentice?> GetApprentice(long apprenticeId) => await _aanDataContext
                .Apprentices
                .AsNoTracking().Where(a => a.ApprenticeId == apprenticeId)
                .SingleOrDefaultAsync();
    }
}
