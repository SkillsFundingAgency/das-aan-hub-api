using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IApprenticesWriteRepository
    {
        Task Create(Apprentice apprentice);

    }
}
