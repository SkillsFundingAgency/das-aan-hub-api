using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IStagedApprenticesReadRepository
    {
        Task<StagedApprentice?> GetStagedApprentice(string lastname, DateTime dateofbirth, string email);
    }
}