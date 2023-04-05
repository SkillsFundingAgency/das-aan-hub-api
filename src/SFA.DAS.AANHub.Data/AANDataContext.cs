using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;

namespace SFA.DAS.AANHub.Data
{
    [ExcludeFromCodeCoverage]
    public class AanDataContext : DbContext, IAanDataContext
    {
        public DbSet<Audit> Audits => Set<Audit>();
        public DbSet<Region> Regions => Set<Region>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Apprentice> Apprentices => Set<Apprentice>();
        public DbSet<Employer> Employers => Set<Employer>();
        public DbSet<Partner> Partners => Set<Partner>();
        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Calendar> Calendars => Set<Calendar>();
        public DbSet<Profile> Profiles => Set<Profile>();
        public AanDataContext(DbContextOptions<AanDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AanDataContext).Assembly);
        }
    }
}
