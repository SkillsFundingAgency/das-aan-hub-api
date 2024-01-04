using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Configuration;

[ExcludeFromCodeCoverage]
public class MemberLeavingReasonConfiguration : IEntityTypeConfiguration<MemberLeavingReason>
{
    public void Configure(EntityTypeBuilder<MemberLeavingReason> builder)
    {
        builder.ToTable("MemberLeavingReason");
        builder.HasKey(x => x.Id);
    }
}