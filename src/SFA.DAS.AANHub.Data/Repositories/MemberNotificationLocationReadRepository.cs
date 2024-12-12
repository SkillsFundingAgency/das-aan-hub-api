using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class MemberNotificationLocationReadRepository : IMemberNotificationLocationReadRepository
{
    private readonly AanDataContext _aanDataContext;
    public MemberNotificationLocationReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public async Task<IEnumerable<MemberNotificationLocation>> GetMemberNotificationLocationsByMember(Guid memberId, CancellationToken cancellationToken) => await _aanDataContext
        .MemberNotificationLocations
        .AsNoTracking()
        .Where(m => m.MemberId == memberId)
        .ToListAsync(cancellationToken);
}