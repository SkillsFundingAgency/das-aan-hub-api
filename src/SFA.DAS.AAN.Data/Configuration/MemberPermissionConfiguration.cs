
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AAN.Domain.Entities;


namespace SFA.DAS.AAN.Data.Configuration
{
    public class MemberPermissionConfiguration : IEntityTypeConfiguration<MemberPermission>
    {
        public void Configure(EntityTypeBuilder<MemberPermission> builder)
        {
            builder.ToTable("MemberPermission");
            builder.HasKey(x => new { x.MemberId, x.PermissionId });
        }
    }
}
