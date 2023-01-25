using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Admins.Queries
{
    public class GetAdminMemberQueryHandler : IRequestHandler<GetAdminMemberQuery, ValidatedResponse<GetAdminMemberResult>>
    {
        private readonly IAdminsReadRepository _adminsReadRepository;

        public GetAdminMemberQueryHandler(IAdminsReadRepository adminsReadRepository) => _adminsReadRepository = adminsReadRepository;

        public async Task<ValidatedResponse<GetAdminMemberResult>> Handle(GetAdminMemberQuery request, CancellationToken cancellationToken)
        {
            var admin = await _adminsReadRepository.GetAdminByUserName(request.UserName);
            return new ValidatedResponse<GetAdminMemberResult>(admin!);
        }
    }
}