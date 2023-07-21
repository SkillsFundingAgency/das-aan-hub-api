using Microsoft.EntityFrameworkCore;
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
            .Include(x => x.Attendees.Where(a => a.IsAttending))
            .ThenInclude(x => x.Member)
            .Include(x => x.EventGuests)
            .Include(x => x.Calendar)
            .SingleOrDefaultAsync();

    public async Task<List<CalendarEventSummary>> GetCalendarEvents(GetCalendarEventsOptions options, CancellationToken cancellationToken)
    {
        var eventFormats = GenerateEventFormatsSql(options.EventFormats);
        var eventTypes = GenerateEventTypesSql(options.CalendarIds);
        var regions = GenerateRegionsSql(options.RegionIds);

        var keywordSql = options.KeywordCount switch
        {
            1 => " ANFREETEXT(Title,'" + options.Keyword?.Trim() + "') ",
            > 1 => " AND CONTAINS(Title,'\"" + options.Keyword?.Trim() + "\"') ",
            _ => string.Empty
        };

        var isActiveSql = string.Empty;
        if (options.IsActive != null)
        {
            isActiveSql = options.IsActive.Value ? " AND CE.IsActive = 1 " : " AND CE.IsActive = 0 ";
        }

        var sql = $@"select	               
 CE.Id as CalendarEventId, 
 COUNT(*) OVER () TotalCount,
 C.CalendarName,
 C.Id as CalendarId,
 CE.RegionId,
 CE.EventFormat, 
 CE.StartDate as [Start], 
 CE.EndDate as [End],
 CE.Title,
 CE.[Description], 
 CE.Summary,
 CE.[Location],   
 CE.Postcode,
 CE.Latitude,
 CE.Longitude,
 CASE   WHEN (EmployerDetails.Latitude is null) THEN null
        WHEN (EmployerDetails.Longitude is null) THEN null
        WHEN (CE.Latitude is null OR CE.Longitude is null) THEN null
    ELSE
    ROUND(geography::Point(CE.Latitude, CE.Longitude, 4326)
    .STDistance(geography::Point(convert(float,EmployerDetails.Latitude), convert(float,EmployerDetails.Longitude), 4326)) * 0.0006213712,1) END
    as Distance,
CONVERT(bit,ISNULL(A.IsAttending, 0)) AS IsAttending,
CE.IsActive,
ISNULL(A.Attendees,0) as NumberOfAttendees
 FROM CalendarEvent CE INNER JOIN Calendar C ON CE.CalendarId = C.Id
 LEFT OUTER JOIN (
    SELECT MemberId,
        MAX(CASE WHEN ProfileId = {Constants.ProfileDataId.Latitude} THEN ProfileValue ELSE null END) Latitude,
        MAX(CASE WHEN ProfileId = {Constants.ProfileDataId.Longitude} THEN ProfileValue ELSE null END) Longitude
    FROM MemberProfile mp1
    WHERE MemberId = '{options.MemberId}'
    GROUP BY MemberId
    ) EmployerDetails on EmployerDetails.MemberId = '{options.MemberId}'
 LEFT outer join (
    SELECT CalendarEventId
          ,MAX(CASE WHEN MemberId = '{options.MemberId}' THEN isAttending ELSE 0 END)  isAttending
          ,SUM(CASE WHEN IsAttending = 1 THEN 1 ELSE 0 END) Attendees 
   FROM Attendance
   GROUP BY CalendarEventid ) A on A.CalendarEventId = CE.Id
 WHERE CE.StartDate >= convert(datetime,'{options.FromDate?.ToString("yyyy-MM-dd HH:mm:ss")}') 
 AND CE.EndDate < convert(date,dateadd(day,1,'{options.ToDate?.ToString("yyyy-MM-dd")}'))
 {isActiveSql}
 {keywordSql}
 {eventFormats}
 {eventTypes}
 {regions}
 Order by CE.StartDate
 OFFSET {(options.Page - 1) * options.PageSize} ROWS 
 FETCH NEXT {options.PageSize} ROWS ONLY";


        var calendarEvents = await _aanDataContext.CalendarEventSummaries!
            .FromSqlRaw(sql)
            .ToListAsync(cancellationToken);
        return calendarEvents;
    }

    private static string GenerateRegionsSql(IReadOnlyCollection<int> regions)
    {
        switch (regions.Count)
        {
            case 0:
                return "";
            case 1:
                return $"AND CE.RegionId = {regions.First()}";
            default:
                var eventTypes = "AND CE.RegionId IN (";
                eventTypes += string.Join(",", regions.ToList());
                eventTypes += ")";
                return eventTypes;
        }
    }

    private static string GenerateEventTypesSql(IReadOnlyCollection<int> calendarIds)
    {
        switch (calendarIds.Count)
        {
            case 0:
                return "";
            case 1:
                return $"AND C.Id = {calendarIds.First()}";
            default:
                var eventTypes = "AND C.Id IN (";
                eventTypes += string.Join(",", calendarIds.ToList());
                eventTypes += ")";
                return eventTypes;
        }
    }

    private static string GenerateEventFormatsSql(IReadOnlyList<EventFormat> eventFormatsList)
    {
        switch (eventFormatsList.Count)
        {
            case 0:
                return "";
            case 1:
                return $"AND CE.EventFormat = '{eventFormatsList[0]}'";
            default:
                var eventFormats = "AND CE.EventFormat IN (";
                eventFormats += string.Join(",", eventFormatsList.Select(ef => "'" + ef + "'").ToList());
                eventFormats += ")";
                return eventFormats;

        }
    }
}