using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Attendances.Queries.GetMemberAttendances;

public class GetMemberAttendancesQueryHandler : IRequestHandler<GetMemberAttendancesQuery, ValidatedResponse<GetMemberAttendancesQueryResult>>
{
    private readonly IAttendancesReadRepository _attendancesReadRepository;

    public GetMemberAttendancesQueryHandler(IAttendancesReadRepository attendancesReadRepository)
    {
        _attendancesReadRepository = attendancesReadRepository;
    }

    public async Task<ValidatedResponse<GetMemberAttendancesQueryResult>> Handle(GetMemberAttendancesQuery request, CancellationToken cancellationToken)
    {
        var attendances = await _attendancesReadRepository.GetAttendances(request.RequestedByMemberId, request.FromDate.GetValueOrDefault().Date, request.ToDate.GetValueOrDefault().Date.AddDays(1), cancellationToken);

        var result = new GetMemberAttendancesQueryResult(attendances.Select(a => (AttendanceModel)a).ToList());

        return new ValidatedResponse<GetMemberAttendancesQueryResult>(result);
    }
}
