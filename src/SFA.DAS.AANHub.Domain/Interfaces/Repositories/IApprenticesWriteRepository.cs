using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IApprenticesWriteRepository
    {
        void Create(Apprentice apprentice);

        Task<Apprentice?> GetPatchApprentice(long apprenticeId);
    }
}