using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Apprentices.Queries
{
    public class GetApprenticeMemberQuery : IRequest<ValidatedResponse<GetApprenticeMemberResult>>
    {
        public GetApprenticeMemberQuery(long apprenticeId) => ApprenticeId = apprenticeId;

        public long ApprenticeId { get; }
    }
}