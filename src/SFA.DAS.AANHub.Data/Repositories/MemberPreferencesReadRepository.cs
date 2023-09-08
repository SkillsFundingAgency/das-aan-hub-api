using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

internal class MemberPreferencesReadRepository : IMemberPreferencesReadRepository
{
    private readonly AanDataContext _aanDataContext;
    public MemberPreferencesReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public async Task<IEnumerable<MemberPreference>?> GetMemberPreferencesByMember(Guid memberId, CancellationToken cancellationToken) => await _aanDataContext
        .MemberPreference
        .AsNoTracking()
        .Where(m => m.MemberId == memberId).ToListAsync(cancellationToken);
}