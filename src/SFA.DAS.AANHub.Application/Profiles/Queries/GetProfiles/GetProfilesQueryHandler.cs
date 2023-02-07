using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesQueryHandler : IRequestHandler<GetProfilesQuery, ValidatedResponse<GetProfilesQueryResult>>
    {
        private readonly IProfilesReadRepository _profilesReadRepository;

        public GetProfilesQueryHandler(IProfilesReadRepository profilesReadRepository) => _profilesReadRepository = profilesReadRepository;

        public async Task<ValidatedResponse<GetProfilesQueryResult>> Handle(GetProfilesQuery request, CancellationToken cancellationToken)
            => new(await _profilesReadRepository.GetAllProfiles());
    }
}
