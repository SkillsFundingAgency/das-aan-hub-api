using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Employers.Queries
{
    public class GetEmployerMemberQuery : IRequest<ValidatableResponse<GetEmployerMemberResult>>
    {
        public long AccountId { get; }
        public long ExternalUserId { get; }
        public GetEmployerMemberQuery(long accountId, long externalUserId)
        {
            AccountId = accountId;
            ExternalUserId = externalUserId;
        }
    }
}
