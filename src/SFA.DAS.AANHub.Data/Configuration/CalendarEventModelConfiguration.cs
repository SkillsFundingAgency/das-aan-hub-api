using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Configuration;

[ExcludeFromCodeCoverage]
public class CalendarEventModelConfiguration : IEntityTypeConfiguration<CalendarEventSummary>
{
    public void Configure(EntityTypeBuilder<CalendarEventSummary> builder)
    {
        builder.HasNoKey();
    }
}
