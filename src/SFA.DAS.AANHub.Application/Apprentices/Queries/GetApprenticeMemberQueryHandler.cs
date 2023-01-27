using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Apprentices.Queries
{
    public class GetApprenticeMemberQueryHandler : IRequestHandler<GetApprenticeMemberQuery, ValidatedResponse<GetApprenticeMemberResult>>
    {
        private readonly IApprenticesReadRepository _apprenticesReadRepository;

        public GetApprenticeMemberQueryHandler(IApprenticesReadRepository apprenticesReadRepository)
            => _apprenticesReadRepository = apprenticesReadRepository;

        public async Task<ValidatedResponse<GetApprenticeMemberResult>> Handle(GetApprenticeMemberQuery request, CancellationToken cancellationToken)
        {
            var apprentice = await _apprenticesReadRepository.GetApprentice(request.ApprenticeId);
            return new ValidatedResponse<GetApprenticeMemberResult>(apprentice!);
        }
    }
}