using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;
public interface IMemberLeavingReasonsWriteRepository
{
    void Create(MemberLeavingReason memberLeavingReason);
}
