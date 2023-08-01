using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Admins.Queries;

public record GetAdminMemberQuery(string Email) : IRequest<ValidatedResponse<GetMemberResult>>;
