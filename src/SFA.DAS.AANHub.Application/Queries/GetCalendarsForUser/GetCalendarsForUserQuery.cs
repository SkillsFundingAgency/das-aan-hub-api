using MediatR;

namespace SFA.DAS.AANHub.Application.Queries.GetCalendarsForUser
{
    public class GetCalendarsForUserQuery : IRequest<GetCalendarsForUserResult>
    {
        public Guid MemberId { get; set; }
    }
}
