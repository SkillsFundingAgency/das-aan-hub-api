using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IProfilesReadRepository
    {
        Task<List<Profile>> GetProfilesByUserType(string userType);
    }
}
