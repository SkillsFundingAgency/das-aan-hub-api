using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SFA.DAS.AAN.Data.Configuration;
using SFA.DAS.AAN.Domain.Configuration;
using SFA.DAS.AAN.Domain.Entities;
using SFA.DAS.AAN.Domain.Interfaces;

namespace SFA.DAS.AAN.Data
{
    public class AanDataContext : DbContext,
        IRegionsContext,
        IMembersContext
    {
        private readonly ApplicationSettings? _configuration;

        public virtual DbSet<Region> Regions { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;

        DbSet<Region> IEntityContext<Region>.Entities => Regions;
        DbSet<Member> IEntityContext<Member>.Entities => Members;

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
