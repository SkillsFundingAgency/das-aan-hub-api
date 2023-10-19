using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.StagedApprentices.Queries
{
    public class GetStagedApprenticeQuery : IRequest<ValidatedResponse<GetStagedApprenticeQueryResult>>
    {
        public string LastName { get; }
        public DateTime DateOfBirth { get; }
        public string Email { get; }
        public GetStagedApprenticeQuery(string lastName, DateTime dateOfBirth, string email)
        {
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Email = email;
        }
    }
}