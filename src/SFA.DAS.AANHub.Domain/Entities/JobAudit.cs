﻿using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class JobAudit
    {
        public int Id { get; set; }
        public string JobName { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Notes { get; set; }
    }
}
