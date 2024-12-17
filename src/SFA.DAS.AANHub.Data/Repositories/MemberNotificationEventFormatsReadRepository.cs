using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories
{
    public class MemberNotificationEventFormatsReadRepository : IMemberNotificationEventFormatsReadRepository
    {
        private readonly AanDataContext _aanDataContext;
        public MemberNotificationEventFormatsReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public async Task<IEnumerable<MemberNotificationEventFormat>> GetMemberNotificationEventFormatsByMember(Guid memberId, CancellationToken cancellationToken) => await _aanDataContext
            .MemberNotificationEventFormats
            .AsNoTracking()
            .Where(m => m.MemberId == memberId)
            .ToListAsync(cancellationToken);
    }
}
