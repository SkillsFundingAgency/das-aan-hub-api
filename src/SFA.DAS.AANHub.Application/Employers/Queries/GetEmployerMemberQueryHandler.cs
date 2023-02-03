using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Employers.Queries
{
    public class GetEmployerMemberQueryHandler : IRequestHandler<GetEmployerMemberQuery, ValidatedResponse<GetEmployerMemberResult>>
    {
        private readonly IEmployersReadRepository _employersReadRepository;

        public GetEmployerMemberQueryHandler(IEmployersReadRepository employersReadRepository) => _employersReadRepository = employersReadRepository;

        public async Task<ValidatedResponse<GetEmployerMemberResult>> Handle(GetEmployerMemberQuery request, CancellationToken cancellationToken)
        {
            var employer = await _employersReadRepository.GetEmployerByUserRef(request.UserRef);
            return new ValidatedResponse<GetEmployerMemberResult>(employer!);
        }
    }
}