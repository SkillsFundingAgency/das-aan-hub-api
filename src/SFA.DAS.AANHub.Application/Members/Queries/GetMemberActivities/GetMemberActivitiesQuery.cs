using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMemberActivities;
public record GetMemberActivitiesQuery(Guid MemberId) : IRequest<ValidatedResponse<GetMemberActivitiesResult>>;
