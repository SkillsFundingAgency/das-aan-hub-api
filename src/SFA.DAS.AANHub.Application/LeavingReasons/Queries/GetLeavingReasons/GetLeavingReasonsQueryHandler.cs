using MediatR;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;

public class GetLeavingReasonsQueryHandler : IRequestHandler<GetLeavingReasonsQuery, List<LeavingCategory>>
{
    private readonly ILeavingReasonsReadRepository _leavingReasonsReadRepository;

    public GetLeavingReasonsQueryHandler(ILeavingReasonsReadRepository leavingReasonsReadRepository) => _leavingReasonsReadRepository = leavingReasonsReadRepository;

    public async Task<List<LeavingCategory>> Handle(GetLeavingReasonsQuery request, CancellationToken cancellationToken)
    {
        var allLeavingReasons = await _leavingReasonsReadRepository.GetAllLeavingReasons(cancellationToken);

        var categories = allLeavingReasons.Select(r => r.Category).Distinct();

        return (from category in categories.OrderBy(c => c)
                let leavingReasons = allLeavingReasons.Where(l => l.Category == category)
                    .Select(r => new LeavingReasonProcessed { Id = r.Id, Description = r.Description, Ordering = r.Ordering })
                    .ToList()
                select new LeavingCategory { Category = category, LeavingReasons = leavingReasons }).ToList();
    }
}
