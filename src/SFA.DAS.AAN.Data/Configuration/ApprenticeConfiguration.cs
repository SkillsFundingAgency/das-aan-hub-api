
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AAN.Domain.Entities;


namespace SFA.DAS.AAN.Data.Configuration
{
    public class ApprenticeConfiguration : IEntityTypeConfiguration<Apprentice>
    {
        public void Configure(EntityTypeBuilder<Apprentice> builder)
        {
            builder.ToTable("Apprentice");
            builder.HasKey(x => x.MemberId);
        }
    }
}
