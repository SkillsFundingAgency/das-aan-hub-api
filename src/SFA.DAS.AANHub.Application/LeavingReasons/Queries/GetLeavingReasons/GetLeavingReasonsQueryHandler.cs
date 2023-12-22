using MediatR;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;

public class GetLeavingReasonsQueryHandler : IRequestHandler<GetLeavingReasonsQuery, GetLeavingReasonsQueryResult>
{
    private readonly ILeavingReasonsReadRepository _leavingReasonsReadRepository;

    public GetLeavingReasonsQueryHandler(ILeavingReasonsReadRepository leavingReasonsReadRepository) => _leavingReasonsReadRepository = leavingReasonsReadRepository;

    public async Task<GetLeavingReasonsQueryResult> Handle(GetLeavingReasonsQuery request, CancellationToken cancellationToken)
    {
        return await _leavingReasonsReadRepository.GetAllLeavingReasons(cancellationToken);
    }
}
