using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class CalendarEventsReadRepository : ICalendarEventsReadRepository
{
	private readonly AanDataContext _aanDataContext;
	public CalendarEventsReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

	public async Task<CalendarEvent?> GetCalendarEvent(Guid id) =>
		await _aanDataContext
			.CalendarEvents
			.AsNoTracking()
			.Where(m => m.Id == id)
			.Include(x => x.Attendees.Where(a => a.IsActive))
			.ThenInclude(x => x.Member)
			.Include(x => x.EventGuests)
			.Include(x => x.Calendar)
			.SingleOrDefaultAsync();

	public async Task<List<CalendarEventSummary>> GetCalendarEvents(Guid memberId, CancellationToken cancellationToken)
	{
		FormattableString sql = $@"select	
                            CE.Id as CalendarEventId, 
	                        C.CalendarName,
	                        CE.EventFormat, 
	                        CE.StartDate as [Start], 
	                        CE.EndDate as [End],
	                        CE.[Description], 
	                        CE.Summary,
	                        CE.[Location],   
	                        CE.Postcode,
	                        CE.Latitude,
	                        CE.Longitude,
	                        CASE  WHEN (EmployerLatitude.ProfileValue is null) THEN null
			                    WHEN (EmployerLongitude.ProfileValue is null) THEN null
			                    WHEN (CE.Latitude is null OR CE.Longitude is null) THEN null
		                    ELSE
			                    geography::Point(CE.Latitude, CE.Longitude, 4326)
					                .STDistance(geography::Point(convert(float,EmployerLatitude.ProfileValue), convert(float,EmployerLongitude.ProfileValue), 4326)) * 0.0006213712 END
					        as Distance,
	                        ISNULL(A.IsActive, 0) AS IsAttending
                            from calendarEvent CE inner join Calendar C on CE.CalendarId = C.Id
                            LEFT OUTER JOIN MemberProfile EmployerLatitude on EmployerLatitude.MemberId = {memberId} and EmployerLatitude.ProfileId = 36
                            LEFT OUTER JOIN MemberProfile EmployerLongitude on EmployerLongitude.MemberId = {memberId} and EmployerLongitude.ProfileId = 37
                            LEFT outer join Attendance A on A.CalendarEventId = CE.Id and A.MemberId = {memberId}
                            WHERE CE.IsActive = 1
                                AND CE.StartDate>=convert(date,getutcdate()) 
                                AND CE.EndDate <= convert(date,DATEADD(year,1,getutcdate()))
	                        Order By CE.StartDate ASC";

		var calendarEvents = await _aanDataContext.CalendarEventSummaries!
			.FromSqlInterpolated(sql)
			.ToListAsync(cancellationToken);
		return calendarEvents;
	}
}
