using System.Diagnostics.CodeAnalysis;
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

    public void DeleteLeavingReasons(Guid memberId)
    {
        var leavingReasonsToRemove = _aanDataContext.MemberLeavingReasons.Where(x => x.MemberId == memberId).ToList();

        if (leavingReasonsToRemove.Any())
        {
            _aanDataContext.MemberLeavingReasons.RemoveRange(leavingReasonsToRemove);
        }
    }
}
