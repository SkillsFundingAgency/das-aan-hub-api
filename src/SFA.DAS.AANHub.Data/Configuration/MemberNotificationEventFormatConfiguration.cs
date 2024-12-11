using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration;

[ExcludeFromCodeCoverage]
public class MemberNotificationEventFormatConfiguration : IEntityTypeConfiguration<MemberNotificationEventFormat>
{
    public void Configure(EntityTypeBuilder<MemberNotificationEventFormat> builder)
    {
        builder.ToTable("MemberNotificationEventFormat");
        builder.HasKey(x => x.Id);
    }
}
