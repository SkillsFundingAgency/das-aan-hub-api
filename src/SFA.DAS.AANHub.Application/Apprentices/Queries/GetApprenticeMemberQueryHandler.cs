using MediatR;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Apprentices.Queries
{
    public class GetApprenticeMemberQueryHandler : IRequestHandler<GetApprenticeMemberQuery, GetApprenticeMemberResult?>
    {
        private readonly IApprenticesReadRepository _apprenticesReadRepository;

        public GetApprenticeMemberQueryHandler(IApprenticesReadRepository apprenticesReadRepository) => _apprenticesReadRepository = apprenticesReadRepository;

        public async Task<GetApprenticeMemberResult?> Handle(GetApprenticeMemberQuery request, CancellationToken cancellationToken)
        {
            var apprentice = await _apprenticesReadRepository.GetApprentice(request.ApprenticeId);
            GetApprenticeMemberResult? result = apprentice!;
            return result;
        }
    }
}
