using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Models;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Application.Constants;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMemberActivities;
public class GetMemberActivitiesQueryHandler : IRequestHandler<GetMemberActivitiesQuery, ValidatedResponse<GetMemberActivitiesQueryResult>>
{
    private readonly IAttendancesReadRepository _attendancesReadRepository;
    private readonly IAuditReadRepository _auditReadRepository;

    public GetMemberActivitiesQueryHandler(IAttendancesReadRepository attendancesReadRepository, IAuditReadRepository auditReadRepository)
    {
        _attendancesReadRepository = attendancesReadRepository;
        _auditReadRepository = auditReadRepository;
    }

    public async Task<ValidatedResponse<GetMemberActivitiesQueryResult>> Handle(GetMemberActivitiesQuery request, CancellationToken cancellationToken)
    {
        DateTime rangeStartDate = DateTime.UtcNow.AddMonths(-1 * RangeDuration.EventsRangePeriod).Date;
        DateTime rangeEndDate = DateTime.UtcNow.AddMonths(RangeDuration.EventsRangePeriod).Date;
        DateTime currentDate = DateTime.UtcNow.Date;

        var memberActivities = await _attendancesReadRepository.GetAttendances(request.MemberId, rangeStartDate, rangeEndDate, cancellationToken);
        var audit = await _auditReadRepository.GetLastAttendanceAuditByMemberId(request.MemberId, cancellationToken);

        GetMemberActivitiesQueryResult getMemberActivitiesQueryResult = new GetMemberActivitiesQueryResult();
        if (audit != null)
        {
            getMemberActivitiesQueryResult.LastSignedUpDate = audit.AuditTime;
        }
        getMemberActivitiesQueryResult.EventsAttended = new EventsModel();
        getMemberActivitiesQueryResult.EventsAttended.EventsDateRange = new DateRangeModel()
        {
            FromDate = rangeStartDate,
            ToDate = currentDate
        };
        getMemberActivitiesQueryResult.EventsAttended.Events = memberActivities.Where(x => x.CalendarEvent.StartDate <= currentDate).AsEnumerable().Select(x => new EventAttendanceModel()
        {
            CalendarEventId = x.CalendarEventId,
            EventDate = x.CalendarEvent.StartDate,
            EventTitle = x.CalendarEvent.Title,
            Urn = x.CalendarEvent.Urn
        }).ToList();

        getMemberActivitiesQueryResult.EventsPlanned = new EventsModel();
        getMemberActivitiesQueryResult.EventsPlanned.EventsDateRange = new DateRangeModel()
        {
            FromDate = currentDate.AddDays(1),
            ToDate = rangeEndDate
        };
        getMemberActivitiesQueryResult.EventsPlanned.Events = memberActivities.Where(x => x.CalendarEvent.StartDate > currentDate).AsEnumerable().Select(x => new EventAttendanceModel()
        {
            CalendarEventId = x.CalendarEventId,
            EventDate = x.CalendarEvent.StartDate,
            EventTitle = x.CalendarEvent.Title,
            Urn = x.CalendarEvent.Urn
        }).ToList();

        return new ValidatedResponse<GetMemberActivitiesQueryResult>(getMemberActivitiesQueryResult!);
    }
}
