using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
public class MemberLeavingReasonsWriteRepository : IMemberLeavingReasonsWriteRepository
{
    private readonly AanDataContext _aanDataContext;

    public MemberLeavingReasonsWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public void Create(MemberLeavingReason memberLeavingReason) => _aanDataContext.MemberLeavingReasons.Add(memberLeavingReason);
}
