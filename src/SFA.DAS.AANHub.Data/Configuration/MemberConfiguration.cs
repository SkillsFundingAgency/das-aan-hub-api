using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration;

[ExcludeFromCodeCoverage]
public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Member");
        builder.HasKey(x => x.Id);
        builder.HasOne(m => m.Region).WithMany(r => r.Members);
        builder.HasMany(m => m.MemberProfiles).WithOne(mp => mp.Member);
        builder.HasMany(m => m.MemberPreferences).WithOne(mp => mp.Member);
        builder.HasMany(m => m.Attendances).WithOne(a => a.Member);
        builder.Property(m => m.FullName).ValueGeneratedOnAddOrUpdate();
        builder.Property(m => m.UserType).HasConversion(new EnumToStringConverter<UserType>());
    }
}
