using SFA.DAS.AAN.Domain.Enums;

namespace SFA.DAS.AAN.Domain.Entities.Audit
{
    public class AuditMetadata
    {
        public Guid EntityId { get; set; }
        public Guid ActionedBy { get; set; }
        public Actions Action { get; set; }
        public string? Resource { get; set; }
    }
}
