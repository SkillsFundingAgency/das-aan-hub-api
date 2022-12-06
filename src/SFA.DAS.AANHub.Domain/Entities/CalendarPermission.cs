
namespace SFA.DAS.AANHub.Domain.Entities
{
    public class CalendarPermission
    {
        public long CalendarId { get; set; }
        public long PermissionId { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool View { get; set; }
        public bool Delete { get; set; }
        public virtual List<MemberPermission> MemberPermissions { get; set; } = new List<MemberPermission>();
    }
}
