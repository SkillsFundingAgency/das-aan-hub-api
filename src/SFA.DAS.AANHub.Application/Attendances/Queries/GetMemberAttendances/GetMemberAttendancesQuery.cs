using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Attendances.Queries.GetMemberAttendances;

public record GetMemberAttendancesQuery(Guid RequestedByMemberId, DateTime? FromDate, DateTime? ToDate)
    : IRequest<ValidatedResponse<GetMemberAttendancesQueryResult>>, IRequestedByMemberId;
