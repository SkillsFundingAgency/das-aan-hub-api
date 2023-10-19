using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.StagedApprentices.Queries;

public class GetStagedApprenticeQueryHandler : IRequestHandler<GetStagedApprenticeQuery, ValidatedResponse<GetStagedApprenticeQueryResult>>
{
    private readonly IStagedApprenticesReadRepository _stagedApprenticesReadRepository;

    public GetStagedApprenticeQueryHandler(IStagedApprenticesReadRepository stagedApprenticesReadRepository)
        => _stagedApprenticesReadRepository = stagedApprenticesReadRepository;

    public async Task<ValidatedResponse<GetStagedApprenticeQueryResult>> Handle(GetStagedApprenticeQuery request, CancellationToken cancellationToken)
    {
        var stagedApprentice = await _stagedApprenticesReadRepository.GetStagedApprentice(request.LastName, request.DateOfBirth, request.Email);
        return new ValidatedResponse<GetStagedApprenticeQueryResult>(stagedApprentice!);
    }
}