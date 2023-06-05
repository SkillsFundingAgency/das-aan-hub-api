﻿using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Common;
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

    public async Task<List<CalendarEventSummary>> GetCalendarEvents(Guid memberId, DateTime fromDate, DateTime toDate, List<EventFormat> eventFormat, List<int> calendarId, CancellationToken cancellationToken)
    {

        FormattableString sql = $@"select	
                            CE.Id as CalendarEventId, 
	                        C.CalendarName,
	                        C.Id as CalendarId,
	                        CE.EventFormat, 
	                        CE.StartDate as [Start], 
	                        CE.EndDate as [End],
	                        CE.[Description], 
	                        CE.Summary,
	                        CE.[Location],   
	                        CE.Postcode,
	                        CE.Latitude,
	                        CE.Longitude,
	                        CASE  WHEN (EmployerDetails.Latitude is null) THEN null
			                    WHEN (EmployerDetails.Longitude is null) THEN null
			                    WHEN (CE.Latitude is null OR CE.Longitude is null) THEN null
		                    ELSE
			                    ROUND( geography::Point(CE.Latitude, CE.Longitude, 4326)
					                .STDistance(geography::Point(convert(float,EmployerDetails.Latitude), convert(float,EmployerDetails.Longitude), 4326)) * 0.0006213712,1) END
					        as Distance,
	                        ISNULL(A.IsActive, 0) AS IsAttending
                            from CalendarEvent CE inner join Calendar C on CE.CalendarId = C.Id
                            LEFT OUTER JOIN (
	                            SELECT MemberId
                                      ,MAX(CASE WHEN ProfileId = {Constants.ProfileDataId.Latitude} THEN ProfileValue ELSE null END) Latitude
                                      ,MAX(CASE WHEN ProfileId = {Constants.ProfileDataId.Longitude} THEN ProfileValue ELSE null END) Longitude
                                FROM MemberProfile mp1
                                WHERE MemberId = {memberId}
                                GROUP BY MemberId
	                            ) EmployerDetails on EmployerDetails.MemberId = {memberId}
                            LEFT outer join Attendance A on A.CalendarEventId = CE.Id and A.MemberId = {memberId}
                            WHERE CE.IsActive = 1
                            AND CE.StartDate >= convert(date,{fromDate}) 
                            AND CE.EndDate < convert(date,dateadd(day,1,{toDate}))";

        var calendarEvents = await _aanDataContext.CalendarEventSummaries!
            .FromSqlInterpolated(sql)
            .Where(x => eventFormat.Select(format => format.ToString()).ToList().Contains(x.EventFormat) || eventFormat.Count == 0)
            .Where(x => calendarId.Select(c => c).ToList().Contains(x.CalendarId) || calendarId.Count == 0)
            .OrderBy(x => x.Start)
            .ToListAsync(cancellationToken);
        return calendarEvents;
    }
}
