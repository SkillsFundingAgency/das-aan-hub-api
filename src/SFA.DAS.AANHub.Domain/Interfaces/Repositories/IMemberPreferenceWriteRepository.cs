using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;
public interface IMemberPreferenceWriteRepository
{
    void Create(MemberPreference member);
}
