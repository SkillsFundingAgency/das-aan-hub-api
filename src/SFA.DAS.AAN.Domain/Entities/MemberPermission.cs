
namespace SFA.DAS.AAN.Domain.Entities
{
    public class MemberPermission
    {
        public Guid MemberId { get; set; }
        public long PermissionId { get; set; }
        public bool IsActive { get; set; }
    }
}
