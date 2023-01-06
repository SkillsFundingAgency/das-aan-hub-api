using MediatR;

namespace SFA.DAS.AANHub.Application.Apprentices.Queries
{
    public class GetApprenticeMemberQuery : IRequest<GetApprenticeMemberResult>
    {
        public long ApprenticeId { get; }

        public GetApprenticeMemberQuery(long apprenticeid)
        {
            ApprenticeId = apprenticeid;
        }
    }
}
