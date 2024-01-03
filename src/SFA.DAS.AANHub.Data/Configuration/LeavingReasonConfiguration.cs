using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Configuration;

[ExcludeFromCodeCoverage]
public class LeavingReasonConfiguration : IEntityTypeConfiguration<LeavingReason>
{
    public void Configure(EntityTypeBuilder<LeavingReason> builder)
    {
        builder.ToTable(nameof(LeavingReason));
        builder.HasKey(x => x.Id);
    }
}
