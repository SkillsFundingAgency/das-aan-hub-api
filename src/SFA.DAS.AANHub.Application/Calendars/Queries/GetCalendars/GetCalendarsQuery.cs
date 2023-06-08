using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Calendars.Queries.GetCalendars;

public class GetCalendarsQuery : IRequest<ValidatedResponse<GetCalendarsQueryResult>>
{
}