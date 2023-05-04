using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Employers.Queries;

public record GetMemberQuery(Guid UserRef) : IRequest<ValidatedResponse<GetMemberResult>>;
