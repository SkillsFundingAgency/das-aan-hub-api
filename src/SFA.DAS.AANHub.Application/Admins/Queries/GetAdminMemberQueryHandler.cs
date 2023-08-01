using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Admins.Queries;

public class GetAdminMemberQueryHandler : IRequestHandler<GetAdminMemberQuery, ValidatedResponse<GetMemberResult>>
{
    private readonly IMembersReadRepository _membersReadRepository;

    public GetAdminMemberQueryHandler(IMembersReadRepository membersReadRepository) => _membersReadRepository = membersReadRepository;

    public async Task<ValidatedResponse<GetMemberResult>> Handle(GetAdminMemberQuery request, CancellationToken cancellationToken)
    {
        var member = await _membersReadRepository.GetMemberByEmail(request.Email);
        return member == null ? ValidatedResponse<GetMemberResult>.EmptySuccessResponse() : new ValidatedResponse<GetMemberResult>(member!);
    }
}