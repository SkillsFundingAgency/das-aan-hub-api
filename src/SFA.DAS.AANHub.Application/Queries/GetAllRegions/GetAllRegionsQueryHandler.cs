using MediatR;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Queries.GetAllRegions
{
    public class GetAllRegionsQueryHandler : IRequestHandler<GetAllRegionsQuery, GetAllRegionsResult>
    {
        private readonly IRegionsReadRepository _regionsReadRepository;

        public GetAllRegionsQueryHandler(IRegionsReadRepository regionsReadRepository) => _regionsReadRepository = regionsReadRepository;

        public async Task<GetAllRegionsResult> Handle(GetAllRegionsQuery request, CancellationToken cancellationToken)
        {
            var regionSummary = await _regionsReadRepository.GetAllRegions();

            return regionSummary.Any()
                ? new GetAllRegionsResult
                {
                    Regions = regionSummary
                }
                : new GetAllRegionsResult { Regions = new List<Region>() };

        }
    }
}
