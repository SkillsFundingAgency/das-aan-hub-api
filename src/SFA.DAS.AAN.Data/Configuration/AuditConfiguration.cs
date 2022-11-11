
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AAN.Domain.Entities;
using SFA.DAS.AAN.Domain.Entities.Audit;

namespace SFA.DAS.AAN.Data.Configuration
{
    public class AuditConfiguration : IEntityTypeConfiguration<AuditData>
    {
        public void Configure(EntityTypeBuilder<AuditData> builder)
        {
            builder.ToTable("Audit");
            builder.HasKey(x => x.Id);
        }
    }
}
