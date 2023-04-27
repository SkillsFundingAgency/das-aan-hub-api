using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Member");
        builder.HasKey(x => x.Id);
        builder.HasOne(m => m.Region).WithMany(r => r.Members);
        builder.HasMany(m => m.MemberProfiles).WithOne(mp => mp.Member);
        builder.HasMany(m => m.Attendances).WithOne(a => a.Member);
    }
}

//public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
//{
//    public void Configure(EntityTypeBuilder<Attendance> builder)
//    {
       
//    }
//}

//public class CalendarEventConfiguration : IEntityTypeConfiguration<CalendarEvent>
//{
//    public void Configure(EntityTypeBuilder<CalendarEvent> builder)
//    {
//        throw new NotImplementedException();
//    }
//}

