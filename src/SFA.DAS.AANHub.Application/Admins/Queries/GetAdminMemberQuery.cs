using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Admins.Queries
{
    public class GetAdminMemberQuery : IRequest<ValidatedResponse<GetAdminMemberResult>>
    {
        public GetAdminMemberQuery(string userName) => UserName = userName;
        public string UserName { get; }
    }
}