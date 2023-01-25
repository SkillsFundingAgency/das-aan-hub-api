using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Employers.Queries
{
    public class GetEmployerMemberQueryHandler : IRequestHandler<GetEmployerMemberQuery, ValidatableResponse<GetEmployerMemberResult>>
    {
        private readonly IEmployersReadRepository _employersReadRepository;

        public GetEmployerMemberQueryHandler(IEmployersReadRepository employersReadRepository) => _employersReadRepository = employersReadRepository;

        public async Task<ValidatableResponse<GetEmployerMemberResult>> Handle(GetEmployerMemberQuery request, CancellationToken cancellationToken)
        {
            var employer = await _employersReadRepository.GetEmployerByUserId(request.UserId);
            return new ValidatableResponse<GetEmployerMemberResult>(employer!);
        }
    }
}
