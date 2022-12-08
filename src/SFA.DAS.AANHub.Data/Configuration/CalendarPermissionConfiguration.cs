using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration
{
    public class CalendarPermissionConfiguration : IEntityTypeConfiguration<CalendarPermission>
    {
        public void Configure(EntityTypeBuilder<CalendarPermission> builder)
        {
            builder.ToTable("CalendarPermission");
            builder.HasKey(x => new { x.CalendarId, x.PermissionId });

        }
    }
}
