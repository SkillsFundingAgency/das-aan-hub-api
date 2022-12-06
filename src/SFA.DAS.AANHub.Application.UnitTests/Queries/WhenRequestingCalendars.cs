using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Queries.GetCalendars;
using SFA.DAS.AANHub.Data;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Queries
{
    public class WhenRequestingCalendars
    {

        private readonly Mock<ICalendarsReadRepository> _calendarsReadRepository;
        private readonly Mock<ICalendarsPermissionsReadRepository> _calendarsPermissionsReadRepository;
        private readonly GetCalendarsQueryHandler _handler;

        public WhenRequestingCalendars()
        {
            _calendarsReadRepository = new Mock<ICalendarsReadRepository>();
            _calendarsPermissionsReadRepository = new Mock<ICalendarsPermissionsReadRepository>();
            _handler = new GetCalendarsQueryHandler(_calendarsReadRepository.Object, _calendarsPermissionsReadRepository.Object);
        }

        [Test, AutoMoqData]
        public async Task ThenAllCalendarsAreReturned(
            GetCalendarsQuery query,
            [Frozen(Matching.ImplementedInterfaces)] AanDataContext context,
            GetCalendarsQueryHandler handler,
            List<Calendar> calendars,
            List<CalendarPermission> permissions)
        {

            context.Calendars.AddRange(calendars);
            context.CalendarPermissions.AddRange(permissions);
            await context.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            result?.Should().NotBeNull();
        }

        [Test, AutoMoqData]
        public async Task ThenAllCalendarsForCreateAreReturned(
            GetCalendarsQuery query)
        {
            var calendar = new Calendar
            {
                CalendarName = "TestCalendar",
                Description = "Description",
                EffectiveFrom = DateTime.Now,
                EffectiveTo = DateTime.Now,
                Id = 1234,
                IsActive = true
            };

            var calendarPermission = new CalendarPermission
            {
                CalendarId = 1234,
                Create = true,
                Delete = true,
                PermissionId = 2345,
                Update = true,
                View = true
            };

            var calendars = new List<Calendar> { calendar };
            var calendarPermissions = new List<CalendarPermission> { calendarPermission };

            _calendarsReadRepository.Setup(m => m.GetAllCalendars()).ReturnsAsync(calendars);
            _calendarsPermissionsReadRepository.Setup(m => m.GetAllCalendarsPermissions()).ReturnsAsync(calendarPermissions);

            var result = await _handler.Handle(query, CancellationToken.None);

            var getCalendarsResultItems = result.ToList();

            getCalendarsResultItems?.Should().NotBeNull();

            //Validate calendar and permissions have been joined correctly

            if (getCalendarsResultItems != null)
            {
                var create = getCalendarsResultItems[0]?.Create?.ToList()[0];
                var update = getCalendarsResultItems[0]?.Update?.ToList()[0];
                var delete = getCalendarsResultItems[0]?.Delete?.ToList()[0];
                var view = getCalendarsResultItems[0]?.View?.ToList()[0];

                create?.Should().Be(2345);
                update?.Should().Be(2345);
                delete?.Should().Be(2345);
                view?.Should().Be(2345);

            }

        }
    }
}
