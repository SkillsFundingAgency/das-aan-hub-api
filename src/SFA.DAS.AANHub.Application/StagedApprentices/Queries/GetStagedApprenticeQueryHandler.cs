using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.StagedApprentices.Queries
{
    public class GetStagedApprenticeMemberQueryHandler : IRequestHandler<GetStagedApprenticeQuery, ValidatedResponse<GetStagedApprenticeResult>>
    {
        private readonly IStagedApprenticesReadRepository _stagedApprenticesReadRepository;

        public GetStagedApprenticeMemberQueryHandler(IStagedApprenticesReadRepository stagedApprenticesReadRepository)
            => _stagedApprenticesReadRepository = stagedApprenticesReadRepository;

        public async Task<ValidatedResponse<GetStagedApprenticeResult>> Handle(GetStagedApprenticeQuery request, CancellationToken cancellationToken)
        {
            var stagedApprentice = await _stagedApprenticesReadRepository.GetStagedApprentice(request.LastName, request.DateOfBirth, request.Email);
            return new ValidatedResponse<GetStagedApprenticeResult>(stagedApprentice!);
        }
    }
}