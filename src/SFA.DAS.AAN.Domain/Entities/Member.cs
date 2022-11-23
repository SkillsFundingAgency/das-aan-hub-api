namespace SFA.DAS.AAN.Domain.Entities
{
    public class Member : EntityBase
    {
        public Guid Id { get; set; }
        public string? UserType { get; set; }
        public string? Status { get; set; }
        public string? Information { get; set; }
        public int? RegionId { get; set; }
        public string? Organisation { get; set; }
        public DateTime Joined { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
