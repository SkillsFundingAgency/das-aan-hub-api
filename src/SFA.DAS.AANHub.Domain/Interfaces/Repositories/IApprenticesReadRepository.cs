using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IApprenticesReadRepository
    {
        Task<Apprentice?> GetApprentice(long apprenticeId);
    }
}