using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMember;

public record GetMemberQuery(Guid MemberId) : IRequest<ValidatedResponse<GetMemberResult>>;
