using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;

namespace SFA.DAS.AANHub.Application.Queries.GetAllRegions
{
    public class GetAllRegionsQueryHandler : IRequestHandler<GetAllRegionsQuery, GetAllRegionsResult>
    {
        private readonly IRegionsContext _regionsContext;

        public GetAllRegionsQueryHandler(IRegionsContext regionsContext)
        {
            _regionsContext = regionsContext;
        }

        public async Task<GetAllRegionsResult> Handle(GetAllRegionsQuery request, CancellationToken cancellationToken)
        {
            var regionSummary = await _regionsContext.Entities.ToListAsync(cancellationToken);

            return regionSummary.Any()
                ? new GetAllRegionsResult
                {
                    Regions = regionSummary
                }
                : new GetAllRegionsResult { Regions = new List<Region>() };

        }
    }
}
