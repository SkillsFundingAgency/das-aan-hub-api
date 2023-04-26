using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class ProfilesReadRepository : IProfilesReadRepository
    {
        private readonly AanDataContext _aanDataContext;

        public ProfilesReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public async Task<List<Profile>> GetProfilesByUserType(string userType) => await _aanDataContext
            .Profiles
            .AsNoTracking()
            .Where(x => x.UserType.Equals(userType))
            .ToListAsync();
    }
}
