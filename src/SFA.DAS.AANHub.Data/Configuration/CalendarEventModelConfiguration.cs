using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Configuration;

[ExcludeFromCodeCoverage]
public class CalendarEventModelConfiguration : IEntityTypeConfiguration<CalendarEventModel>
{
    public void Configure(EntityTypeBuilder<CalendarEventModel> builder)
    {
        builder.HasNoKey();
    }
}
