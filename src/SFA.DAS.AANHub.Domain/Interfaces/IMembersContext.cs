using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Domain.Interfaces
{

    public interface IMembersContext : ISaveableEntityContext<Member>
    {
        [ExcludeFromCodeCoverage]
        public async Task<Member?> FindByIdAsync(Guid memberId)
            => await Entities.FirstOrDefaultAsync(x => x.Id == memberId);

    }
}
