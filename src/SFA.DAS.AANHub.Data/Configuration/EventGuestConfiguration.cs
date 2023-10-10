using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Configuration;

[ExcludeFromCodeCoverage]
public class EventGuestConfiguration : IEntityTypeConfiguration<EventGuest>
{
    public void Configure(EntityTypeBuilder<EventGuest> builder)
    {
        builder.ToTable("EventGuest");
        builder.HasKey(x => x.Id);
    }
}