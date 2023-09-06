using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMemberByEmail;

public class GetMemberByEmailQueryHandler : IRequestHandler<GetMemberByEmailQuery, ValidatedResponse<GetMemberResult>>
{
    private readonly IMembersReadRepository _membersReadRepository;

    public GetMemberByEmailQueryHandler(IMembersReadRepository membersReadRepository) => _membersReadRepository = membersReadRepository;

    public async Task<ValidatedResponse<GetMemberResult>> Handle(GetMemberByEmailQuery request, CancellationToken cancellationToken)
    {
        var member = await _membersReadRepository.GetMemberByEmail(request.Email);
        return member == null ? ValidatedResponse<GetMemberResult>.EmptySuccessResponse() : new ValidatedResponse<GetMemberResult>(member!);
    }
}