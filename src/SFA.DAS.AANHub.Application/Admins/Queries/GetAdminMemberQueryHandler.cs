using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Admins.Queries
{
    public class GetAdminMemberQueryHandler : IRequestHandler<GetAdminMemberQuery, ValidatedResponse<GetMemberResult>>
    {
        private readonly IAdminsReadRepository _adminsReadRepository;

        public GetAdminMemberQueryHandler(IAdminsReadRepository adminsReadRepository) => _adminsReadRepository = adminsReadRepository;

        public async Task<ValidatedResponse<GetMemberResult>> Handle(GetAdminMemberQuery request, CancellationToken cancellationToken)
        {
            var admin = await _adminsReadRepository.GetAdminByUserName(request.UserName);
            return admin == null ? ValidatedResponse<GetMemberResult>.EmptySuccessResponse() : new ValidatedResponse<GetMemberResult>(admin.Member!);
        }
    }
}