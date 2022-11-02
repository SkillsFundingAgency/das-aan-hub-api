using Microsoft.EntityFrameworkCore;

namespace SFA.DAS.AAN.Domain.Interfaces
{
    public interface IEntityContext<T> where T : class
    {
        DbSet<T> Entities { get; }
    }
}
