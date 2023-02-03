using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Employers.Queries
{
    public class GetEmployerMemberQuery : IRequest<ValidatedResponse<GetEmployerMemberResult>>
    {
        public GetEmployerMemberQuery(Guid userRef) => UserRef = userRef;

        public Guid UserRef { get; }
    }
}