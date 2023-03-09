using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Apprentices.Queries
{
    public class GetApprenticeMemberQuery : IRequest<ValidatedResponse<GetApprenticeMemberResult>>
    {
        public GetApprenticeMemberQuery(Guid apprenticeId) => ApprenticeId = apprenticeId;

        public Guid ApprenticeId { get; }
    }
}