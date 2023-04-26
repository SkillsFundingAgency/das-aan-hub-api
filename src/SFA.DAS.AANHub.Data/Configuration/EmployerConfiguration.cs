using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Data.Configuration;

public class EmployerConfiguration : IEntityTypeConfiguration<Employer>
{
    public void Configure(EntityTypeBuilder<Employer> builder)
    {
        builder.ToTable("Employer");
        builder.HasKey(x => x.MemberId);
    }
}
