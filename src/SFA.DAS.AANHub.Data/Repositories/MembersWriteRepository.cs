using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class MembersWriteRepository : IMembersWriteRepository
{
    private readonly AanDataContext _aanDataContext;

    public MembersWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public void Create(Member member) => _aanDataContext.Members.Add(member);

    public async Task<Member?> Get(Guid id) =>
        await _aanDataContext.Members.Where(m => m.Id == id)
            .Include(x => x.MemberProfiles)
            .Include(x => x.MemberPreferences)
            .Include(x => x.MemberNotificationEventFormats)
            .Include(x => x.MemberNotificationLocations)
            .SingleOrDefaultAsync();
}
