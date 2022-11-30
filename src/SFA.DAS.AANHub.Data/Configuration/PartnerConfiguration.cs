using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration
{
    public class PartnerConfiguration : IEntityTypeConfiguration<Partner>
    {
        public void Configure(EntityTypeBuilder<Partner> builder)
        {
            builder.ToTable("Partner");
            builder.HasKey(x => x.MemberId);
        }
    }
}
