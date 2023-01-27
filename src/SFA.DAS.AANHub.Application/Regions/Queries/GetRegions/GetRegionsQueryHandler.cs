using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Regions.Queries.GetRegions
{
    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, ValidatedResponse<GetRegionsQueryResult>>
    {
        private readonly IRegionsReadRepository _regionsReadRepository;

        public GetRegionsQueryHandler(IRegionsReadRepository regionsReadRepository) => _regionsReadRepository = regionsReadRepository;

        public async Task<ValidatedResponse<GetRegionsQueryResult>> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
            => new(await _regionsReadRepository.GetAllRegions());
    }
}