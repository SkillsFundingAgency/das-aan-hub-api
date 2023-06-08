using MediatR;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Calendars.Queries.GetCalendars;

public class GetCalendarsQueryHandler : IRequestHandler<GetCalendarsQuery, GetCalendarsQueryResult>
{
    private readonly ICalendarsReadRepository _calendarsReadRepository;

    public GetCalendarsQueryHandler(ICalendarsReadRepository calendarsReadRepository) => _calendarsReadRepository = calendarsReadRepository;

    public async Task<GetCalendarsQueryResult> Handle(GetCalendarsQuery request, CancellationToken cancellationToken)
    {
        return await _calendarsReadRepository.GetAllCalendars(cancellationToken);
    }
}
