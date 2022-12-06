using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class MembersWriteRepository : IMembersWriteRepository
    {
        private readonly AanDataContext _aanDataContext;

        public MembersWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public async Task Create(Member member)
        {
            _aanDataContext.Members.Add(member);
            await _aanDataContext.SaveChangesAsync();
        }
    }
}
