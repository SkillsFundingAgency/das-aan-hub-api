using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Partners.Queries
{
    public class GetPartnerMemberQuery : IRequest<ValidatedResponse<GetPartnerMemberResult>>
    {
        public string UserName { get; }

        public GetPartnerMemberQuery(string userName)
        {
            UserName = userName;
        }
    }
}
