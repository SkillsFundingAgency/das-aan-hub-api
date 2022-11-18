
using MediatR;
using System.Security.Cryptography.X509Certificates;

namespace SFA.DAS.AAN.Application.Queries.GetCalendarsForUser
{
    public class GetCalendarsForUserQuery : IRequest<GetCalendarsForUserResult>
    {
        public Guid MemberId { get; set; }
    }
}
