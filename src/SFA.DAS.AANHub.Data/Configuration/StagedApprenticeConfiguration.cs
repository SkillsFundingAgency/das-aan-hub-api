using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration;

public class StagedApprenticeConfiguration : IEntityTypeConfiguration<StagedApprentice>
{
    public void Configure(EntityTypeBuilder<StagedApprentice> builder)
    {
        builder.ToTable("StagedApprentice");
        builder.HasKey(x => x.Id);
    }
}