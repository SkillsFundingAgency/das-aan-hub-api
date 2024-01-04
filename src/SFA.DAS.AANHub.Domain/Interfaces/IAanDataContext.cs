using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces;

public interface IAanDataContext
{
    DbSet<Audit> Audits { get; }
    DbSet<Region> Regions { get; }
    DbSet<Member> Members { get; }
    DbSet<Apprentice> Apprentices { get; }
    DbSet<Employer> Employers { get; }
    DbSet<StagedApprentice> StagedApprentices { get; }
    DbSet<Calendar> Calendars { get; }
    DbSet<Profile> Profiles { get; }
    DbSet<CalendarEvent> CalendarEvents { get; }
    DbSet<Attendance> Attendances { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<NotificationTemplate> NotificationTemplates { get; }
    DbSet<MemberPreference> MemberPreferences { get; }

    DbSet<EventGuest> EventGuests { get; }
    DbSet<LeavingReason> LeavingReasons { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
