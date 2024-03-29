﻿using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Apprentices.Queries.GetApprenticeMember;

public record GetApprenticeMemberQuery(Guid ApprenticeId) : IRequest<ValidatedResponse<GetMemberResult>>;
