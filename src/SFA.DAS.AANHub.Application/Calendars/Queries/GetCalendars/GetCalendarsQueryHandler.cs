using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Calendars.Queries.GetCalendars;

public class GetCalendarsQueryHandler : IRequestHandler<GetCalendarsQuery, ValidatedResponse<GetCalendarsQueryResult>>
{
    private readonly ICalendarsReadRepository _calendarsReadRepository;

    public GetCalendarsQueryHandler(ICalendarsReadRepository calendarsReadRepository) => _calendarsReadRepository = calendarsReadRepository;

    public async Task<ValidatedResponse<GetCalendarsQueryResult>> Handle(GetCalendarsQuery request, CancellationToken cancellationToken)
            => new(await _calendarsReadRepository.GetAllCalendars(cancellationToken));
}