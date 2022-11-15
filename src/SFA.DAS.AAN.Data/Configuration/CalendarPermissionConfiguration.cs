
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AAN.Domain.Entities;


namespace SFA.DAS.AAN.Data.Configuration
{
    public class CalendarPermissionConfiguration : IEntityTypeConfiguration<CalendarPermission>
    {
        public void Configure(EntityTypeBuilder<CalendarPermission> builder)
        {
            builder.ToTable("CalendarPermission");
            builder.HasKey(x => x.CalendarId);
        }
    }
}
