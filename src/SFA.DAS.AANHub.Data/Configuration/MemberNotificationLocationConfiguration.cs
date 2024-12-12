using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration;

[ExcludeFromCodeCoverage]
public class MemberNotificationLocationConfiguration : IEntityTypeConfiguration<MemberNotificationLocation>
{
    public void Configure(EntityTypeBuilder<MemberNotificationLocation> builder)
    {
        builder.ToTable("MemberNotificationLocation");
        builder.HasKey(x => x.Id);
    }
}
