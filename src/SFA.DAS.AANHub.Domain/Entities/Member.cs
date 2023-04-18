using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; set; }
        public string? UserType { get; set; }
        public string? Status { get; set; }
        public string? Information { get; set; }
        public DateTime Joined { get; set; }
        public int? RegionId { get; set; }

        public virtual Region? Region { get; set; }
        [JsonIgnore]
        public virtual Admin? Admin { get; set; }
        [JsonIgnore]
        public virtual Apprentice? Apprentice { get; set; }
        [JsonIgnore]
        public virtual Employer? Employer { get; set; }
        [JsonIgnore]
        public virtual Partner? Partner { get; set; }
    }
}