using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data
{
    [ExcludeFromCodeCoverage]
    public class AanDataContext : DbContext, IAanDataContext
    {
        public DbSet<Region> Regions { get; set; } = null!;
        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<Apprentice> Apprentices { get; set; } = null!;
        public DbSet<Employer> Employers { get; set; } = null!;
        public DbSet<Partner> Partners { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; } = null!;
        public DbSet<Calendar> Calendars { get; set; } = null!;
        public DbSet<CalendarPermission> CalendarPermissions { get; set; } = null!;
        public DbSet<MemberPermission> MemberPermissions { get; set; } = null!;
        public AanDataContext(DbContextOptions<AanDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AanDataContext).Assembly);
        }
    }
}
