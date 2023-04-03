using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class StagedApprenticesReadRepository : IStagedApprenticesReadRepository
    {
        private readonly AanDataContext _aanDataContext;

        public StagedApprenticesReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public async Task<StagedApprentice?> GetStagedApprentice(string lastname, DateTime dateofbirth, string email) => await _aanDataContext
                .StagedApprentice
                .Where(a => a.LastName == lastname && a.DateOfBirth == dateofbirth && a.Email == email)
                .AsNoTracking()
                .SingleOrDefaultAsync();
    }
}