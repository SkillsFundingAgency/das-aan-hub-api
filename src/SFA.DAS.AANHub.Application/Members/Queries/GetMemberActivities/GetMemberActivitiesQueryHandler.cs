using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Models;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Application.Constants;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMemberActivities;
public class GetMemberActivitiesQueryHandler : IRequestHandler<GetMemberActivitiesQuery, ValidatedResponse<GetMemberActivitiesResult>>
{
    private readonly IAttendancesReadRepository _attendancesReadRepository;
    private readonly IAuditReadRepository _auditReadRepository;

    public GetMemberActivitiesQueryHandler(IAttendancesReadRepository attendancesReadRepository, IAuditReadRepository auditReadRepository)
    {
        _attendancesReadRepository = attendancesReadRepository;
        _auditReadRepository = auditReadRepository;
    }

    public async Task<ValidatedResponse<GetMemberActivitiesResult>> Handle(GetMemberActivitiesQuery request, CancellationToken cancellationToken)
    {
        DateTime rangeStartDate = DateTime.UtcNow.AddMonths(-1 * RangeDuration.EventsRangePeriod);
        DateTime rangeEndDate = DateTime.UtcNow.AddMonths(RangeDuration.EventsRangePeriod);
        DateTime currentDate = DateTime.UtcNow;

        var memberActivities = await _attendancesReadRepository.GetAttendances(request.MemberId, rangeStartDate, rangeEndDate, cancellationToken);
        var audit = await _auditReadRepository.GetLastAttendanceAuditByMemberId(request.MemberId, cancellationToken);

        GetMemberActivitiesResult getMemberActivitiesResult = new GetMemberActivitiesResult();
        if (audit != null)
        {
            getMemberActivitiesResult.LastSignedUpDate = audit.AuditTime;
        }
        getMemberActivitiesResult.EventsAttended = new EventsModel();
        getMemberActivitiesResult.EventsAttended.EventsDateRange = new DateRangeModel()
        {
            FromDate = rangeStartDate,
            ToDate = currentDate
        };
        getMemberActivitiesResult.EventsAttended.Events = memberActivities.Where(x => x.AddedDate <= currentDate).AsEnumerable().Select(x => new EventAttendanceModel()
        {
            CalendarEventId = x.CalendarEventId,
            EventDate = x.AddedDate,
            EventTitle = x.CalendarEvent.Title,
            Urn = x.CalendarEvent.Urn
        }).ToList();

        getMemberActivitiesResult.EventsPlanned = new EventsModel();
        getMemberActivitiesResult.EventsPlanned.EventsDateRange = new DateRangeModel()
        {
            FromDate = currentDate.AddDays(1),
            ToDate = rangeEndDate
        };
        getMemberActivitiesResult.EventsPlanned.Events = memberActivities.Where(x => x.AddedDate > currentDate).AsEnumerable().Select(x => new EventAttendanceModel()
        {
            CalendarEventId = x.CalendarEventId,
            EventDate = x.AddedDate,
            EventTitle = x.CalendarEvent.Title,
            Urn = x.CalendarEvent.Urn
        }).ToList();

        return new ValidatedResponse<GetMemberActivitiesResult>(getMemberActivitiesResult!);
    }
}
