using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class StagedApprenticesReadRepository : IStagedApprenticesReadRepository
{
    private readonly AanDataContext _aanDataContext;

    public StagedApprenticesReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public async Task<StagedApprentice?> GetStagedApprentice(string lastName, DateTime dateOfBirth, string email)
    {
        var query = _aanDataContext
            .StagedApprentices
            .Where(a => a.LastName == lastName && a.DateOfBirth == dateOfBirth && a.Email == email);

        return await query.AsNoTracking().SingleOrDefaultAsync();
    }
}