using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMemberByEmail;

public record GetMemberByEmailQuery(string Email) : IRequest<ValidatedResponse<GetMemberByEmailResult>>;