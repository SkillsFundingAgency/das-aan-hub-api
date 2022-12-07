using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration
{
    public class AuditConfiguration : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.ToTable("Audit");
            builder.Property(a => a.ActionedBy).IsRequired();
            builder.HasOne(a => a.Member)
                .WithMany(m => m.Audits)
                .HasForeignKey(a => a.ActionedBy);
        }
    }

}
