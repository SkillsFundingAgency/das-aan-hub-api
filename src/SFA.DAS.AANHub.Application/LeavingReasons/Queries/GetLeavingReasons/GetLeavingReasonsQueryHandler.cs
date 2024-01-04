using MediatR;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;

public class GetLeavingReasonsQueryHandler : IRequestHandler<GetLeavingReasonsQuery, IEnumerable<LeavingCategory>>
{
    private readonly ILeavingReasonsReadRepository _leavingReasonsReadRepository;

    public GetLeavingReasonsQueryHandler(ILeavingReasonsReadRepository leavingReasonsReadRepository) => _leavingReasonsReadRepository = leavingReasonsReadRepository;

    public async Task<IEnumerable<LeavingCategory>> Handle(GetLeavingReasonsQuery request, CancellationToken cancellationToken)
    {
        var allLeavingReasons = await _leavingReasonsReadRepository.GetAllLeavingReasons(cancellationToken);

        return allLeavingReasons.GroupBy(r => r.Category)
            .Select(g => new LeavingCategory(g.Key, g.Select(x => (LeavingReasonModel)x)));
    }
}
