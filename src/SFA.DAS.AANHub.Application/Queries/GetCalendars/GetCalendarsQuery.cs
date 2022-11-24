
using MediatR;

namespace SFA.DAS.AANHub.Application.Queries.GetCalendars
{
    public class GetCalendarsQuery : IRequest<IEnumerable<GetCalendarsResultItem>>
    {
    }
}
