
using MediatR;


namespace SFA.DAS.AAN.Application.Queries.GetCalendars
{
    public class GetCalendarsQuery : IRequest<IEnumerable<GetCalendarsResultItem>>
    {
    }
}
