using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.StagedApprentices.Queries
{
    public class GetStagedApprenticeQuery : IRequest<ValidatedResponse<GetStagedApprenticeResult>>
    {
        public GetStagedApprenticeQuery(string lastname, DateTime dateofbirth, string email)
        {
            LastName = lastname;
            DateOfBirth = dateofbirth;
            Email = email;
        }
        public string LastName { get; }
        public DateTime DateOfBirth { get; }
        public string Email { get; }
    }
}