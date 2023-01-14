using MediatR;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Regions.Queries
{
    public class GetAllRegionsQueryHandler : IRequestHandler<GetAllRegionsQuery, GetAllRegionsQueryResult>
    {
        private readonly IRegionsReadRepository _regionsReadRepository;

        public GetAllRegionsQueryHandler(IRegionsReadRepository regionsReadRepository) => _regionsReadRepository = regionsReadRepository;

        public async Task<GetAllRegionsQueryResult> Handle(GetAllRegionsQuery request, CancellationToken cancellationToken)
        {
            var regionSummary = await _regionsReadRepository.GetAllRegions();

            return new GetAllRegionsQueryResult
            {
                Regions = regionSummary
            };

        }
    }
}
