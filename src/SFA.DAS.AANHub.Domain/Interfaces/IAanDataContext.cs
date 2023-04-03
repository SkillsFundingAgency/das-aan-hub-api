using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Domain.Interfaces
{
    public interface IAanDataContext
    {
        DbSet<Audit> Audits { get; }
        DbSet<Region> Regions { get; }
        DbSet<Member> Members { get; }
        DbSet<Apprentice> Apprentices { get; }
        DbSet<Employer> Employers { get; }
        DbSet<Partner> Partners { get; }
        DbSet<Admin> Admins { get; }
        DbSet<Calendar> Calendars { get; }
        DbSet<Profile> Profiles { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
