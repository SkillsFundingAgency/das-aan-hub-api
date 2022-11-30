using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration
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
