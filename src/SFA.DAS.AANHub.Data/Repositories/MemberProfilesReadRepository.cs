using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

internal class MemberProfilesReadRepository : IMemberProfilesReadRepository
{
    private readonly AanDataContext _aanDataContext;

    public MemberProfilesReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public async Task<IEnumerable<MemberProfile>?> GetMemberProfilesByMember(Guid memberId, CancellationToken cancellationToken) => await _aanDataContext
        .MemberProfile
        .AsNoTracking()
        .Where(m => m.MemberId == memberId).Include(x => x.Profile)//.ThenInclude(x => x.Preference)
        .ToListAsync(cancellationToken);
}
