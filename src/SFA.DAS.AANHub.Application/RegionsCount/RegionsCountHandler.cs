using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.RegionsCount;
public class RegionsCountHandler : IRequestHandler<RegionsCountRequest, int>
{
    private readonly IRegionsReadRepository _regionsReadRepository;
    private readonly ILogger<RegionsCountHandler> _logger;
    public RegionsCountHandler(IRegionsReadRepository regionsReadRepository, ILogger<RegionsCountHandler> logger)
    {
        _regionsReadRepository = regionsReadRepository;
        _logger = logger;
    }

    public async Task<int> Handle(RegionsCountRequest request, CancellationToken cancellationToken)
    {
        var regionsCount = await _regionsReadRepository.GetRegionsCount(cancellationToken);
        _logger.LogInformation("Gathering regions count:{regionsCount}", regionsCount);
        return regionsCount;
    }
}
