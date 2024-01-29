using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMemberActivities;
public record GetMemberActivitiesQuery(Guid MemberId) : IRequest<ValidatedResponse<GetMemberActivitiesQueryResult>>, IMemberId;
