using Microsoft.EntityFrameworkCore;
using SFA.DAS.AAN.Domain.Entities;


namespace SFA.DAS.AAN.Domain.Interfaces
{
    public interface IMembersContext : ISaveableEntityContext<Member>
    {
        public async Task<Member?> FindByIdAsync(Guid memberId)
            => await Entities.FirstOrDefaultAsync(x => x.Id == memberId);

    }
}
