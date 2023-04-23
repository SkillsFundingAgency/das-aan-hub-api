using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Employers.Queries;

public class GetEmployerMemberQueryHandler : IRequestHandler<GetEmployerMemberQuery, ValidatedResponse<GetMemberResult>>
{
    private readonly IEmployersReadRepository _employersReadRepository;

    public GetEmployerMemberQueryHandler(IEmployersReadRepository employersReadRepository) => _employersReadRepository = employersReadRepository;

    public async Task<ValidatedResponse<GetMemberResult>> Handle(GetEmployerMemberQuery request, CancellationToken cancellationToken)
    {
        var employer = await _employersReadRepository.GetEmployerByUserRef(request.UserRef);
        return employer == null ? ValidatedResponse<GetMemberResult>.EmptySuccessResponse() : new ValidatedResponse<GetMemberResult>(employer.Member!);
    }
}