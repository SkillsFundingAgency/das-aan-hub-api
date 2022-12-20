using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration
{
    public class MemberRegionConfiguration : IEntityTypeConfiguration<MemberRegion>
    {
        public void Configure(EntityTypeBuilder<MemberRegion> builder)
        {
            builder.ToTable("MemberRegion");
            builder.HasKey(x => new { x.MemberId, x.RegionId });
        }
    }
}
