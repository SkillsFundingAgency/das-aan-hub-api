using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Employers.Queries;

public class GetMemberQueryHandler : IRequestHandler<GetMemberQuery, ValidatedResponse<GetMemberResult>>
{
    private readonly IMembersReadRepository _membersReadRepository;

    public GetMemberQueryHandler(IMembersReadRepository membersReadRepository) => _membersReadRepository = membersReadRepository;

    public async Task<ValidatedResponse<GetMemberResult>> Handle(GetMemberQuery request, CancellationToken cancellationToken)
    {
        var member = await _membersReadRepository.GetMember(request.UserRef);
        return member == null ? ValidatedResponse<GetMemberResult>.EmptySuccessResponse() : new ValidatedResponse<GetMemberResult>(member);
    }
}