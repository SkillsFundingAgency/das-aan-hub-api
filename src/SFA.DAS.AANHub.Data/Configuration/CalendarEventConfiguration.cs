
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;


namespace SFA.DAS.AANHub.Data.Configuration
{
    public class CalendarEventConfiguration : IEntityTypeConfiguration<CalendarEvent>
    {
        public void Configure(EntityTypeBuilder<CalendarEvent> builder)
        {
            builder.ToTable("CalendarEvent");
            builder.HasKey(x => x.Id);
        }
    }
}
