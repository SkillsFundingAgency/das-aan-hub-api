namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories
{
    public interface IMembersPermissionsReadRepository
    {
        Task<List<long>> GetAllMemberPermissionsForUser(Guid id);
    }
}
