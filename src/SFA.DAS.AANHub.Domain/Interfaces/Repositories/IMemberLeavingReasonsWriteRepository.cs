using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces.Repositories;
public interface IMemberLeavingReasonsWriteRepository
{
    void CreateMemberLeavingReasons(List<MemberLeavingReason> memberLeavingReasons);
}
