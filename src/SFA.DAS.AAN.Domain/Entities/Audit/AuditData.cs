using System.Text.Json.Nodes;
using SFA.DAS.AAN.Domain.Enums;

namespace SFA.DAS.AAN.Domain.Entities.Audit
{
    public class AuditData
    {
        public Guid? Id { get; set; }
        public Guid EntityId { get; set; }
        public DateTime AuditTime { get; set; }
        public Guid ActionedBy { get; set; }
        public string? Action { get; set; }
        public string? Resource { get; set; }
        public string? Before { get; set; }
        public string? After { get; set; }
    }
}
