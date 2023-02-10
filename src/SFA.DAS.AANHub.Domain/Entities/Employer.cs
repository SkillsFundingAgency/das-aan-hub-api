﻿namespace SFA.DAS.AANHub.Domain.Entities
{
    public class Employer
    {
        public Guid MemberId { get; set; }
        public long AccountId { get; set; }
        public Guid UserRef { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Organisation { get; set; } = null!;
        public DateTime LastUpdated { get; set; }
    }
}