using MediatR;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesByUserTypeQueryHandler : IRequestHandler<GetProfilesByUserTypeQuery, GetProfilesByUserTypeQueryResult>
    {
        private readonly IProfilesReadRepository _profilesReadRepository;

        public GetProfilesByUserTypeQueryHandler(IProfilesReadRepository profilesReadRepository) => _profilesReadRepository = profilesReadRepository;

        public async Task<GetProfilesByUserTypeQueryResult> Handle(GetProfilesByUserTypeQuery request, CancellationToken cancellationToken)
        {
            var profiles = await _profilesReadRepository.GetProfilesByUserType(request.UserType);
            var models = profiles.Select(p => (ProfileModel)p).ToList();
            return models;
        }
    }
}
