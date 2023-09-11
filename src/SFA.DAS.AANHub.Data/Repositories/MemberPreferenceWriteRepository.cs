using System.Diagnostics.CodeAnalysis;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class MemberPreferenceWriteRepository : IMemberPreferenceWriteRepository
{
    private readonly AanDataContext _aanDataContext;

    public MemberPreferenceWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public void Create(MemberPreference memberPreference) => _aanDataContext.MemberPreferences.Add(memberPreference);
}