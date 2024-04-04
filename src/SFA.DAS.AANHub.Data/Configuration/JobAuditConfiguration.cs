using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class JobAuditConfiguration : IEntityTypeConfiguration<JobAudit>
    {
        public void Configure(EntityTypeBuilder<JobAudit> builder)
        {
            builder.ToTable("JobAudit");
            builder.HasKey(x => x.Id);
        }
    }
}
