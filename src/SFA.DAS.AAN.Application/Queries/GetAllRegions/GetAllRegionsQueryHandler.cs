using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AAN.Domain.Entities;
using SFA.DAS.AAN.Domain.Interfaces;

namespace SFA.DAS.AAN.Application.Queries.GetAllRegions
{
    public class GetAllRegionsQueryHandler : IRequestHandler<GetAllRegionsQuery, GetAllRegionsResult>
    {
        private readonly IMembersContext _membersContext;

        public GetAllRegionsQueryHandler(IMembersContext membersContext)
        {
            _membersContext = membersContext;
        }

        public async Task<GetAllRegionsResult> Handle(GetAllRegionsQuery request, CancellationToken cancellationToken)
        {
            var regionSummary = await _membersContext.Entities.ToListAsync(cancellationToken);

            return regionSummary.Any()
                ? new GetAllRegionsResult
                {
                    Regions = regionSummary
                }
                : new GetAllRegionsResult { Regions = new List<Region>() };

        }
    }
}
