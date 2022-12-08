using MediatR;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Queries.GetCalendars
{
    public class GetCalendarsQueryHandler : IRequestHandler<GetCalendarsQuery, IEnumerable<GetCalendarsResultItem>>
    {
        private readonly ICalendarsReadRepository _calendarsReadRepository;
        private readonly ICalendarsPermissionsReadRepository _calendarsPermissionsReadRepository;

        public GetCalendarsQueryHandler(ICalendarsReadRepository calendarsReadRepository, ICalendarsPermissionsReadRepository calendarsPermissionsReadRepository)
        {
            _calendarsReadRepository = calendarsReadRepository;
            _calendarsPermissionsReadRepository = calendarsPermissionsReadRepository;
        }

        public async Task<IEnumerable<GetCalendarsResultItem>> Handle(GetCalendarsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Calendar> calendars = await _calendarsReadRepository.GetAllCalendars();
            IEnumerable<CalendarPermission> permissions = await _calendarsPermissionsReadRepository.GetAllCalendarsPermissions();

            var items =
                calendars.Select(c => new GetCalendarsResultItem()
                {
                    Calendar = c.CalendarName,
                    CalendarId = c.Id,
                    Create = permissions.Where(x => x.CalendarId == c.Id && x.Create)
                                        .Select(x => x.PermissionId),
                    Update = permissions.Where(x => x.CalendarId == c.Id && x.Update)
                                        .Select(x => x.PermissionId),
                    View = permissions.Where(x => x.CalendarId == c.Id && x.View)
                                        .Select(x => x.PermissionId),
                    Delete = permissions.Where(x => x.CalendarId == c.Id && x.Delete)
                                        .Select(x => x.PermissionId)
                });

            return items;
        }
    }
}
