using MediatR;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Queries.GetApprentice
{
    public class GetApprenticeMemberQueryHandler : IRequestHandler<GetApprenticeMemberQuery, GetApprenticeMemberResult>
    {
        private readonly IApprenticesReadRepository _apprenticesReadRepository;

        public GetApprenticeMemberQueryHandler(IApprenticesReadRepository apprenticesReadRepository) => _apprenticesReadRepository = apprenticesReadRepository;

        public async Task<GetApprenticeMemberResult> Handle(GetApprenticeMemberQuery request, CancellationToken cancellationToken)
        {
            var result = await _apprenticesReadRepository.GetApprentice(request.ApprenticeId);

            return result;
        }
    }
}
