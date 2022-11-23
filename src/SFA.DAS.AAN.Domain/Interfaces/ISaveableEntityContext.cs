namespace SFA.DAS.AAN.Domain.Interfaces
{
    public interface ISaveableEntityContext<T> : IEntityContext<T> where T : class
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
