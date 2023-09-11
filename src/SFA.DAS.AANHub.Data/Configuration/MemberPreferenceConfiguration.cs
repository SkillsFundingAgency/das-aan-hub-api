using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration;

[ExcludeFromCodeCoverage]
public class MemberPreferenceConfiguration : IEntityTypeConfiguration<MemberPreference>
{
    public void Configure(EntityTypeBuilder<MemberPreference> builder)
    {
        builder.ToTable("MemberPreference");
        builder.HasKey(x => x.Id);
    }
}
