using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Configuration;

[ExcludeFromCodeCoverage]
public class ApprenticeConfiguration : IEntityTypeConfiguration<Apprentice>
{
    public void Configure(EntityTypeBuilder<Apprentice> builder)
    {
        builder.ToTable("Apprentice");
        builder.HasKey(x => x.MemberId);
    }
}
