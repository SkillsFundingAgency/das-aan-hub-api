
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AAN.Domain.Entities;


namespace SFA.DAS.AAN.Data.Configuration
{
    public class CalendarConfiguration : IEntityTypeConfiguration<Calendar>
    {
        public void Configure(EntityTypeBuilder<Calendar> builder)
        {
            builder.ToTable("Calendar");
            builder.HasKey(x => x.Id);
        }
    }
}
