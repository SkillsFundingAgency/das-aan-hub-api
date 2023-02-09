using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Partners.Queries
{
    public class GetPartnerMemberQueryHandler : IRequestHandler<GetPartnerMemberQuery, ValidatedResponse<GetPartnerMemberResult>>
    {
        private readonly IPartnersReadRepository _partnersReadRepository;

        public GetPartnerMemberQueryHandler(IPartnersReadRepository partnersReadRepository) => _partnersReadRepository = partnersReadRepository;

        public async Task<ValidatedResponse<GetPartnerMemberResult>> Handle(GetPartnerMemberQuery request, CancellationToken cancellationToken)
        {
            var partner = await _partnersReadRepository.GetPartnerByUserName(request.UserName);
            return new ValidatedResponse<GetPartnerMemberResult>(partner!); 
        }
    }
}
