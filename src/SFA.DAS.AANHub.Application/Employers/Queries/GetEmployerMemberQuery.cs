using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Employers.Queries
{
    public class GetEmployerMemberQuery : IRequest<ValidatableResponse<GetEmployerMemberResult>>
    {
        public long UserId { get; }

        public GetEmployerMemberQuery(long userId)
        {
            UserId = userId;
        }
    }
}
