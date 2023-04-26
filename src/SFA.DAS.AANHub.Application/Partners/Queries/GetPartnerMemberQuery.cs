using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Partners.Queries;

public record GetPartnerMemberQuery(string UserName) : IRequest<ValidatedResponse<GetMemberResult>>;
