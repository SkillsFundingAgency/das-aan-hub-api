using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration;

[ExcludeFromCodeCoverage]
public class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
{
    public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
    {
        builder.ToTable("NotificationTemplate");
        builder.HasKey(x => x.Id);
    }
}