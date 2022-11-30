using Microsoft.EntityFrameworkCore;

namespace SFA.DAS.AANHub.Domain.Interfaces
{
    public interface IEntityContext<T> where T : class
    {
        DbSet<T> Entities { get; }
    }
}
