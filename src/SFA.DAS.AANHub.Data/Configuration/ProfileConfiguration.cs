using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profile");
        builder.HasKey(x => x.Id);

    }
}
