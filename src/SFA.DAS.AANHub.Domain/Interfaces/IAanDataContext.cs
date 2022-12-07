using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces
{
    public interface IAanDataContext
    {
        DbSet<Region> Regions { get; set; }
        DbSet<Member> Members { get; set; }
        DbSet<Apprentice> Apprentices { get; set; }
        DbSet<Employer> Employers { get; set; }
        DbSet<Partner> Partners { get; set; }
        DbSet<Admin> Admins { get; set; }
        DbSet<Calendar> Calendars { get; set; }
        DbSet<CalendarPermission> CalendarPermissions { get; set; }
        DbSet<MemberPermission> MemberPermissions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
