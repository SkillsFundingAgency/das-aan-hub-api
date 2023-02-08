using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesQueryHandler : IRequestHandler<GetProfilesQuery, ValidatedResponse<List<ProfileModel>>>
    {
        private readonly IProfilesReadRepository _profilesReadRepository;

        public GetProfilesQueryHandler(IProfilesReadRepository profilesReadRepository) => _profilesReadRepository = profilesReadRepository;

        public async Task<ValidatedResponse<List<ProfileModel>>> Handle(GetProfilesQuery request, CancellationToken cancellationToken)
        {
            var profiles = await _profilesReadRepository.GetAllProfiles(request.UserType);
            var models = profiles.Select(p => (ProfileModel)p).ToList();
            return new ValidatedResponse<List<ProfileModel>>(models);
        }

    }
}
