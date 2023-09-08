using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;

namespace SFA.DAS.AANHub.Data;

[ExcludeFromCodeCoverage]
public class AanDataContext : DbContext, IAanDataContext
{
    public DbSet<Audit> Audits => Set<Audit>();
    public DbSet<Region> Regions => Set<Region>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Apprentice> Apprentices => Set<Apprentice>();
    public DbSet<Employer> Employers => Set<Employer>();
    public DbSet<StagedApprentice> StagedApprentices => Set<StagedApprentice>();
    public DbSet<Calendar> Calendars => Set<Calendar>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<CalendarEvent> CalendarEvents => Set<CalendarEvent>();
    public DbSet<CalendarEventSummary>? CalendarEventSummaries { get; set; }
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();

    public AanDataContext(DbContextOptions<AanDataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AanDataContext).Assembly);
    }
}
