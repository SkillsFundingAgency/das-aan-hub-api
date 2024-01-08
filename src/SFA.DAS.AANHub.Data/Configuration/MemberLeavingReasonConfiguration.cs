using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration;

[ExcludeFromCodeCoverage]
public class MemberLeavingReasonConfiguration : IEntityTypeConfiguration<MemberLeavingReason>
{
    public void Configure(EntityTypeBuilder<MemberLeavingReason> builder)
    {
        builder.ToTable(nameof(MemberLeavingReason));
        builder.HasKey(x => x.Id);
    }
}