﻿using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; set; }
        public string? UserType { get; set; }
        public string? Status { get; set; }
        public string? ReviewStatus { get; set; }
        public string? Information { get; set; }
        public DateTime Joined { get; set; }
        [JsonIgnore]
        public virtual Admin? Admin { get; set; }
        [JsonIgnore]
        public virtual Apprentice? Apprentice { get; set; }
        [JsonIgnore]
        public virtual Employer? Employer { get; set; }
        [JsonIgnore]
        public virtual Partner? Partner { get; set; }
        public virtual List<MemberRegion>? MemberRegions { get; set; }

        public static List<MemberRegion> GenerateMemberRegions(List<int>? regions, Guid id)
        {
            var memberRegions = new List<MemberRegion>();
            if (regions == null) return memberRegions;

            memberRegions.AddRange(regions.Select(region => new MemberRegion
            {
                MemberId = id,
                RegionId = region
            }));

            return memberRegions;
        }
    }
}