
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SFA.DAS.AAN.Data.Configuration;
using SFA.DAS.AAN.Domain.Configuration;
using SFA.DAS.AAN.Domain.Entities;
using SFA.DAS.AAN.Domain.Entities.Audit;
using SFA.DAS.AAN.Domain.Interfaces;
using Calendar = SFA.DAS.AAN.Domain.Entities.Calendar;


namespace SFA.DAS.AAN.Data
{
    public class AanDataContext : DbContext,
        IRegionsContext,
        IMembersContext,
        IApprenticesContext,
        IEmployersContext,
        IPartnersContext,
        IAdminsContext,
        ICalendarsContext,
        ICalendarPermissionsContext,
        IMemberPermissionsContext,
        IAuditContext
    {
        private readonly ApplicationSettings? _configuration;

        public virtual DbSet<Region> Regions { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<Apprentice> Apprentices { get; set; } = null!;
        public virtual DbSet<Employer> Employers { get; set; } = null!;
        public virtual DbSet<Partner> Partners { get; set; } = null!;
        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Calendar> Calendars { get; set; } = null!;
        public virtual DbSet<CalendarPermission> CalendarPermissions { get; set; } = null!;
        public virtual DbSet<MemberPermission> MemberPermissions { get; set; } = null!;
        public virtual DbSet<AuditData> AuditData { get; set; } = null!;

        DbSet<Region> IEntityContext<Region>.Entities => Regions;
        DbSet<Member> IEntityContext<Member>.Entities => Members;
        DbSet<Apprentice> IEntityContext<Apprentice>.Entities => Apprentices;
        DbSet<Employer> IEntityContext<Employer>.Entities => Employers;
        DbSet<Partner> IEntityContext<Partner>.Entities => Partners;
        DbSet<Admin> IEntityContext<Admin>.Entities => Admins;
        DbSet<Calendar> IEntityContext<Calendar>.Entities => Calendars;
        DbSet<CalendarPermission> IEntityContext<CalendarPermission>.Entities => CalendarPermissions;
        DbSet<MemberPermission> IEntityContext<MemberPermission>.Entities => MemberPermissions;
        DbSet<AuditData> IEntityContext<AuditData>.Entities => AuditData;

        public AanDataContext(DbContextOptions<AanDataContext> options) : base(options)
        {
        }

        public AanDataContext(IOptions<ApplicationSettings> config, DbContextOptions<AanDataContext> options) : base(options)
        {
            _configuration = config.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_configuration == null)
            {
                return;
            }

            var connection = new SqlConnection
            {
                ConnectionString = _configuration.DbConnectionString
            };

            optionsBuilder.UseSqlServer(connection);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RegionConfiguration());
            modelBuilder.ApplyConfiguration(new MemberConfiguration());
            modelBuilder.ApplyConfiguration(new ApprenticeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployerConfiguration());
            modelBuilder.ApplyConfiguration(new PartnerConfiguration());
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new CalendarConfiguration());
            modelBuilder.ApplyConfiguration(new CalendarPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new MemberPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new AuditConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default
        )
        {
            OnBeforeSaving();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess,
                cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                // for entities that inherit from BaseEntity,
                // set UpdatedOn / CreatedOn appropriately
                if (entry.Entity is EntityBase trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            // set the updated date to "now"
                            trackable.Updated = utcNow;

                            // mark property as "don't touch"
                            // we don't want to update on a Modify operation
                            entry.Property("Created").IsModified = false;
                            break;

                        case EntityState.Added:
                            // set both updated and created date to "now"
                            trackable.Created = utcNow;
                            trackable.Updated = utcNow;
                            break;
                    }

                }
            }
        }
    }
}
