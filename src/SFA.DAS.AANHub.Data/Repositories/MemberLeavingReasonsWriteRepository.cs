using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class MemberLeavingReasonsWriteRepository : IMemberLeavingReasonsWriteRepository
{
    private readonly AanDataContext _aanDataContext;

    public MemberLeavingReasonsWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public void CreateMemberLeavingReasons(List<MemberLeavingReason> memberLeavingReasons)
    {
        _aanDataContext.MemberLeavingReasons.AddRange(memberLeavingReasons);
    }

    public async Task DeleteLeavingReasons(Guid memberId, CancellationToken cancellationToken)
    {
        var leavingReasonsToRemove = await _aanDataContext.MemberLeavingReasons.Where(x => x.MemberId == memberId).ToListAsync(cancellationToken);
        _aanDataContext.MemberLeavingReasons.RemoveRange(leavingReasonsToRemove);
    }
}
