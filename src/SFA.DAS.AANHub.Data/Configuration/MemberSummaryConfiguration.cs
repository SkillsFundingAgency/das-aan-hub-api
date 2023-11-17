using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration;

public class MemberSummaryConfiguration : IEntityTypeConfiguration<MemberSummary>
{
    public void Configure(EntityTypeBuilder<MemberSummary> builder)
    {
        builder.Property(m => m.UserType).HasConversion(new EnumToStringConverter<UserType>());
    }
}
